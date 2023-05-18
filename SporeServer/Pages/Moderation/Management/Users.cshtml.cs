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
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Pages.Moderation.Management
{
    [Authorize(Roles = "Admin,Moderator")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAssetManager _assetManager;

        public UsersModel(UserManager<SporeServerUser> userManager, IUserSubscriptionManager userSubscriptionManager, IAssetManager assetManager)
        {
            _userManager = userManager;
            _userSubscriptionManager = userSubscriptionManager;
            _assetManager = assetManager;
        }

        /// <summary>
        ///     Current User
        /// </summary>
        public SporeServerUser CurrentUser { get; set; }
        /// <summary>
        ///     Search Results
        /// </summary>
        public SporeServerUser[] Users { get; set; }
        /// <summary>
        ///     Search String
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        ///     Whether it actually performed the search
        /// </summary>
        public bool Searched { get; set; }

        /// <summary>
        ///     Whether user is in specified role or not
        /// </summary>
        public async Task<bool> IsUserInRoleAsync(SporeServerUser user, string role)
        {
            return (await _userManager.GetRolesAsync(user)).Contains(role);
        }

        public async Task<IActionResult> OnGet()
        {
            SearchString = Request.Query["searchText"];

            CurrentUser = await _userManager.GetUserAsync(User);

            if (String.IsNullOrEmpty(SearchString))
            {
                Users = await _userManager.Users
                            .Where(u => u.UserName != null)
                            .OrderBy(u => u.UserName)
                            .Take(25)
                            .ToArrayAsync();
            }
            else
            {
                Users = await _userManager.Users
                               .Where(u =>
                                       u.UserName != null &&
                                       (
                                           (u.Id.ToString() == SearchString) ||
                                           (u.UserName.Contains(SearchString)) ||
                                           (u.Email == SearchString)
                                       )
                                   )
                               .OrderBy(u => u.UserName)
                               .Take(25)
                               .ToArrayAsync();

            }

            string actionQuery = Request.Query["action"];
            if (!String.IsNullOrEmpty(actionQuery))
            {
                if (actionQuery == "Create")
                {
                    return Redirect("https://spore.com/Moderation/Management/User/Create");
                }
            }

            return Page();
        }
    }
}
