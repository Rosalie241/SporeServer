/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Middleware;
using SporeServer.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAssetManager, AssetManager>();
            services.AddScoped<IAssetCommentManager, AssetCommentManager>();
            services.AddScoped<IUserSubscriptionManager, UserSubscriptionManager>();
            services.AddScoped<IAggregatorManager, AggregatorManager>();
            services.AddScoped<IAggregatorSubscriptionManager, AggregatorSubscriptionManager>();
            services.AddScoped<IEventManager, EventManager>();
            services.AddScoped<IAchievementManager, AchievementManager>();
            services.AddScoped<ILeaderboardManager, LeaderboardManager>();
            services.AddScoped<IRatingManager, RatingManager>();
            services.AddScoped<IBlockedUserManager, BlockedUserManager>();

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | HttpLoggingFields.RequestQuery | HttpLoggingFields.ResponseStatusCode;
            });

            services.AddControllers();
            services.AddRazorPages();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<SporeServerRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<SporeServerUser>>();
            string[] roleNames = { "Admin", "Moderator" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new SporeServerRole(roleName));
                }
            }

            string userId = Configuration["AppSettings:AdminUserId"];
            if (String.IsNullOrEmpty(userId))
            {
                return;
            }

            var adminUser = await userManager.FindByIdAsync(userId);
            if (adminUser != null)
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        private async Task CreateAdminUser(IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<SporeServerUser>>();

            string userId = Configuration["AppSettings:AdminUserId"];
            string email  = Configuration["AppSettings:AdminUserEmail"];
            string userName = Configuration["AppSettings:AdminUserName"];
            string password = Configuration["AppSettings:AdminUserPassword"];

            if (String.IsNullOrEmpty(email) ||
                String.IsNullOrEmpty(userName) ||
                String.IsNullOrEmpty(password))
            {
                return;
            }

            IdentityResult result;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            { // fallback to using email as identifier
                user = await userManager.FindByEmailAsync(email);
            }

            if (user != null && 
                (String.IsNullOrEmpty(user.UserName) ||
                 String.IsNullOrEmpty(user.PasswordHash)))
            {
                user.UserName = userName;
                user.Email    = email;

                result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                { // add password when updating user succeeded
                    result = await userManager.AddPasswordAsync(user, password);
                }

                if (result.Succeeded)
                { // add user to role when adding password succeeds
                    result = await userManager.AddToRoleAsync(user, "Admin");
                }    

                if (result.Succeeded)
                {
                    logger.LogInformation("Successfully registered admin user");
                }
                else
                {
                    logger.LogInformation($"Failed to register admin user: {String.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            // Spore doesn't accept commas or such internally,
            // so let's globally just use the InvariantCulture
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // ensure database is created and migrated
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SporeServerContext>();
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<SporeAuthMiddleware>();

            app.UseHttpsRedirection();

            app.UseHsts();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            if (Configuration["AppSettings:EnableHTTPLogging"] == "1")
            {
                app.UseHttpLogging();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            // create user roles
            CreateUserRoles(serviceProvider).Wait();

            // create admin user
            CreateAdminUser(serviceProvider, logger).Wait();
        }
    }
}

