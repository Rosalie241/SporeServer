/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;

[assembly: HostingStartup(typeof(SporeServer.Areas.Identity.IdentityHostingStartup))]
namespace SporeServer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<SporeServerContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("SporeServerContextConnection"),
                        new MariaDbServerVersion(new Version(10, 5, 9))
                ));

                services.AddIdentity<SporeServerUser, IdentityRole<Int64>>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    // TODO
                    options.SignIn.RequireConfirmedAccount = false;


                }).AddEntityFrameworkStores<SporeServerContext>().AddDefaultTokenProviders();

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.SlidingExpiration = true;

                    // Disable login page redirect
                    options.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.FromResult(0);
                        }
                    };
                });
            });
        }
    }
}