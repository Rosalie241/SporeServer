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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SporeServer;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Middleware;
using SporeServer.Services;
using System;
using System.Globalization;
using System.Net;
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Spore doesn't accept commas or such internally,
            // so let's globally just use the InvariantCulture
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

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

            app.UseStatusCodePages(async context =>
            {
                Console.WriteLine($"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                Console.WriteLine(context.HttpContext.Response.StatusCode);
                await Task.Delay(0);
            });
        }
    }
}

