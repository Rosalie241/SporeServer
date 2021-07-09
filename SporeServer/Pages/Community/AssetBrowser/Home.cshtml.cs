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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Data;
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using SporeServer.Services;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAggregatorManager _aggregatorManager;
        private readonly IAssetManager _assetManager;

        public HomeModel(UserManager<SporeServerUser> userManager, IUserSubscriptionManager userSubscriptionManager, IAggregatorManager aggregatorManager, IAssetManager assetManager)
        {
            _userManager = userManager;
            _userSubscriptionManager = userSubscriptionManager;
            _aggregatorManager = aggregatorManager;
            _assetManager = assetManager;
        }

        /// <summary>
        ///     Subscriber Count
        /// </summary>
        public Int32 SubscribeCount { get; set; }
        /// <summary>
        ///     Aggregator Count
        /// </summary>
        public Int32 AggregatorCount { get; set; }
        /// <summary>
        ///     Asset Count
        /// </summary>
        public Int32 AssetCount { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            SubscribeCount = await _userSubscriptionManager.GetCountByUserAsync(user);
            AggregatorCount = await _aggregatorManager.GetCountByAuthorAsync(user);
            AssetCount = await _assetManager.GetCountByAuthorAsync(user);

            return Page();
        }
    }
}
