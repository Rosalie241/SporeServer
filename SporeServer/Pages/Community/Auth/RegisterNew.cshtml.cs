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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Models;

namespace SporeServer.Pages.Community.Auth
{
    [AllowAnonymous]
    public class RegisterNewModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;

        public RegisterNewModel(UserManager<SporeServerUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] RegisterNewQuery query)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            IdentityResult result;

            // check if first user has been used already,
            // if not, update the user
            var firstUser = await _userManager.FindByIdAsync("1");
            if (firstUser != null && String.IsNullOrEmpty(firstUser.UserName))
            {
                firstUser.UserName = query.DisplayName;
                firstUser.Email = query.Email;

                result = await _userManager.UpdateAsync(firstUser);
                if (result.Succeeded)
                { // add password when updating user succeeded
                    result = await _userManager.AddPasswordAsync(firstUser, query.Password);
                }
            }
            else
            { // first user has already been used, so create a new one
                result = await _userManager.CreateAsync(
                    new SporeServerUser
                    {
                        UserName = query.DisplayName,
                        Email = query.Email
                    }, query.Password);
            }

            return Redirect($"https://community.spore.com/community/auth/registerNew?success={result.Succeeded}");
        }
    }
}
