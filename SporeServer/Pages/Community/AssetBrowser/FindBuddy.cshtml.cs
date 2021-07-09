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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class FindBuddyModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAssetManager _assetManager;

        public FindBuddyModel(UserManager<SporeServerUser> userManager, IUserSubscriptionManager userSubscriptionManager, IAssetManager assetManager)
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
        ///     User Subscribed Users list
        /// </summary>
        public SporeServerUser[] SubscribedUsers { get; set; }
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
        ///     returns Asset Count of given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetAssetCountByUser(Int64 userId)
        {
            var assets = _assetManager.FindAllByUserIdAsync(userId).Result;
            return assets == null ? 0 : assets.Length;
        }
        
        public async Task <IActionResult> OnGet()
        {
            string searchString = Request.Query["searchText"];

            if (String.IsNullOrEmpty(searchString))
            {
                Searched = false;
                return Page();
            }

            CurrentUser = await _userManager.GetUserAsync(User);

            Users = await _userManager.Users
                            .Where(u => 
                                    u.UserName != null &&
                                    (
                                        (u.UserName.Contains(searchString)) ||
                                        (u.Email == searchString)
                                    )
                                )
                            .OrderBy(u => u.UserName)
                            .Take(25)
                            .ToArrayAsync();

            var userSubscriptions = await _userSubscriptionManager.FindAllByAuthorAsync(CurrentUser);
            SubscribedUsers = userSubscriptions
                                        .Select(s => s.User)
                                        .ToArray();

            SearchString = searchString;
            Searched = true;

            return Page();
        }
    }
}
