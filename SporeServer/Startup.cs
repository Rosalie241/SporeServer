using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SporeServer;
using SporeServer.Areas.Identity.Data;
using System;
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
            services.AddControllers();
            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                //options.Cookie = true;
               // options.ExpireTimeSpan = TimeSpan.FromHours(5);

                options.LoginPath = "";
                options.AccessDeniedPath = "";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // TODO:
            // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });


            app.UseStatusCodePages(async context =>
            {
                Console.WriteLine($"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");
                Console.WriteLine(context.HttpContext.Response.StatusCode);
            });
        }
    }
}

