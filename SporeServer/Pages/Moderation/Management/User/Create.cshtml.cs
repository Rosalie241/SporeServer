/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Pages.Moderation.Management.User
{
    [Authorize(Roles = "Admin,Moderator")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;

        public CreateModel(UserManager<SporeServerUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Whether to show the big background or not
        /// </summary>
        public bool ShowBigBackground { get; set; }
        /// <summary>
        ///     Result of user creation
        /// </summary>
        public string ErrorMessage { get; set; } 

        public async Task<IActionResult> OnGet()
        {
            ShowBigBackground = false;

            // parse AppResolution header
            var resolution = Request.Headers["AppResolution"].FirstOrDefault();
            if (!String.IsNullOrEmpty(resolution))
            {
                // make sure it's in the right format
                var splitResolution = resolution.Split('x');

                if (splitResolution.Length == 2)
                {
                    // the official server
                    // only checks the width,
                    // so let's do the same here
                    if (Int32.TryParse(splitResolution[0], out int width))
                    {
                        // show big background
                        // and increase page size
                        // when the resolution width
                        // is large enough
                        if (width >= 1024)
                        {
                            ShowBigBackground = true;
                        }
                    }
                }
            }

            // parse action query
            string actionQuery = Request.Query["action"];
            if (!String.IsNullOrEmpty(actionQuery))
            {
                if (actionQuery == "Back")
                {
                    return Redirect("https://spore.com/Moderation/Management/Users");
                }
                else if (actionQuery == "Create")
                {
                    string emailQuery    = Request.Query["email"];
                    string userNameQuery = Request.Query["userName"];
                    string passwordQuery = Request.Query["password"];
                    string userRoleQuery = Request.Query["userRole"];

                    if (String.IsNullOrEmpty(emailQuery) ||
                        String.IsNullOrEmpty(userNameQuery) ||
                        String.IsNullOrEmpty(passwordQuery))
                    {
                        return Page();
                    }

                    // check if user role is valid
                    if (userRoleQuery != "USER" &&
                        userRoleQuery != "MODERATOR" &&
                        userRoleQuery != "ADMIN")
                    {
                        return Page();
                    }

                    IdentityResult result;

                    var user = new SporeServerUser()
                    {
                        UserName = userNameQuery,
                        Email = emailQuery
                    };

                    result = await _userManager.CreateAsync(user, passwordQuery);

                    // ensure creating the user was successful before
                    // adding the created user to the specified role
                    if (result == IdentityResult.Success && 
                        userRoleQuery != "USER")
                    {
                        string role;
                        if (userRoleQuery == "MODERATOR")
                        {
                            role = "Moderator";
                        }
                        else
                        {
                            role = "Admin";
                        }

                        result = await _userManager.AddToRoleAsync(user, role);
                    }

                    bool success = result == IdentityResult.Success;

                    if (success)
                    {
                        ErrorMessage = "Successfully created user.";
                    }
                    else
                    {
                        ErrorMessage = "Failed to create user.";
                    }
                }
            }

            return Page();
        }
    }
}
