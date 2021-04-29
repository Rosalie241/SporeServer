/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SporeServer.Middleware
{
    public class SporeAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SporeAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<SporeServerUser> userManager, SignInManager<SporeServerUser> signInManager)
        {
            string email, password;

            // we don't need to do anything if
            //  * we're already authenticated
            //  * the header doesn't contain Spore-User (or it's empty)
            //  * the header doesn't contain Spore-Password (or it's empty)
            if (context.User.Identity.IsAuthenticated ||
                !context.Request.Headers.ContainsKey("Spore-User") ||
                String.IsNullOrEmpty(email = context.Request.Headers["Spore-User"]) ||
                !context.Request.Headers.ContainsKey("Spore-Password") ||
                String.IsNullOrEmpty((password = context.Request.Headers["Spore-Password"])))
            {
                await _next(context);
                return;
            }

            // decode email & password
            email = WebUtility.UrlDecode(email);
            password = WebUtility.UrlDecode(password);

            // attempt to find the user
            SporeServerUser user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                // attempt to login using password
                SignInResult result = await signInManager.PasswordSignInAsync(user, password, false, true);
                if (result == SignInResult.Success)
                {
                    // authenticate current context aswell
                    context.User = await signInManager.CreateUserPrincipalAsync(user);
                }
            }

            await _next(context);
        }
    }
}
