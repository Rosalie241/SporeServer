using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
                SignInResult result = await signInManager.PasswordSignInAsync(user, password, false, false);
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
