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
    public class FindSporecastModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAggregatorManager _aggregatorManager;
        private readonly IAggregatorSubscriptionManager _aggregatorSubscriptionManager;

        public FindSporecastModel(UserManager<SporeServerUser> userManager, IAggregatorManager aggregatorManager, IAggregatorSubscriptionManager aggregatorSubscriptionManager)
        {
            _userManager = userManager;
            _aggregatorManager = aggregatorManager;
            _aggregatorSubscriptionManager = aggregatorSubscriptionManager;
        }

        /// <summary>
        ///     Search String
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        ///     Amount Of Filtered Items In Total
        /// </summary>
        public Int32 SearchCount { get; set; }
        /// <summary>
        ///     Searched Aggregators
        /// </summary>
        public SporeServerAggregator[] Aggregators { get; set; }
        /// <summary>
        ///     User's Aggregators
        /// </summary>
        public SporeServerAggregator[] UserAggregators { get; set; }
        /// <summary>
        ///     User's aggregator subscriptions
        /// </summary>
        public Int64[] AggregatorSubscriptions { get; set; }
        /// <summary>
        ///     Current Index
        /// </summary>
        public Int32 CurrentIndex { get; set; }
        /// <summary>
        ///     Next Index
        /// </summary>
        public Int32 NextIndex { get; set; }
        /// <summary>
        ///     Previous Index
        /// </summary>
        public Int32 PreviousIndex { get; set; }

        public async Task<IActionResult> OnGet(Int32? index)
        {
            Console.WriteLine($"/community/assetBrowser/findSporecast/{index}{Request.QueryString}");

            string searchString = Request.Query["searchText"];

            SearchString = searchString;

            bool searchName = !String.IsNullOrEmpty(searchString);

            // search aggregators using LINQ
            var aggregators = _aggregatorManager.GetAllAggregators()
                                    .Include(a => a.Author)
                                    .Include(a => a.Assets)
                                    .Where(a => 
                                        (
                                            (!searchName) ||
                                            (
                                                (a.Name.Contains(searchString)) ||
                                                (a.Author.UserName.Contains(searchString))
                                            )
                                        )
                                    ).OrderBy(a => a.Name);

            CurrentIndex = index ?? 0;
            NextIndex = CurrentIndex + 3;
            PreviousIndex = CurrentIndex - 3;

            SearchCount = await aggregators
                                .CountAsync();

            Aggregators = await aggregators
                                .Skip(CurrentIndex)
                                .Take(3)
                                .ToArrayAsync();

            var author = await _userManager.GetUserAsync(User);

            UserAggregators = await _aggregatorManager.FindByAuthorAsync(author);

            var aggregatorSubscriptions = await _aggregatorSubscriptionManager.FindAllByAuthorAsync(author);
            AggregatorSubscriptions = aggregatorSubscriptions.Select(s => s.AggregatorId)
                                                                .ToArray();
            return Page();
        }
    }
}
