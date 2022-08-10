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
using SporeServer.Data;
using SporeServer.Services;
using SporeServer.SporeTypes;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class CreateSporecastModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;
        private readonly IAggregatorManager _aggregatorManager;

        public CreateSporecastModel(UserManager<SporeServerUser> userManager, IAssetManager assetManager, IAggregatorManager aggregatorManager )
        {
            _userManager = userManager;
            _assetManager = assetManager;
            _aggregatorManager = aggregatorManager;
        }


        /// <summary>
        ///     Search String
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        ///     Filter String
        /// </summary>
        public string FilterString { get; set; }
        /// <summary>
        ///     Amount Of Filtered Items In Total
        /// </summary>
        public Int32 SearchCount { get; set; }
        /// <summary>
        ///     Searched Assets
        /// </summary>
        public SporeServerAsset[] Assets { get; set; }
        /// <summary>
        ///     Amount of Assets
        /// </summary>
        public Int32 AssetCount { get; set; }
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
            string searchString = Request.Query["searchText"];
            string filterString = Request.Query["filter"];

            SearchString = searchString;
            FilterString = filterString;

            bool searchName = !String.IsNullOrEmpty(searchString);
            bool searchModelType = false, searchAssetType = false;

            // if we can parse filterString, make sure we can use it
            if (Int64.TryParse(filterString, out Int64 filterId))
            {
                searchModelType = Enum.IsDefined(typeof(SporeModelType), filterId);
                searchAssetType = Enum.IsDefined(typeof(SporeAssetType), filterId);
            }

            // search the assets using LINQ
            var queryAssets = _assetManager.GetAllAssets()
                                       .Include(a => a.Author)
                                       .Where(a => a.Used &&
                                                    (
                                                        (
                                                            (!searchName) ||
                                                            (
                                                                a.Author.UserName.Contains(searchString) ||
                                                                a.Name.Contains(searchString)
                                                            )
                                                        )
                                                        &&
                                                        (
                                                            (searchModelType && a.ModelType == (SporeModelType)filterId) ||
                                                            (searchAssetType && a.Type == (SporeAssetType)filterId) ||
                                                            (!searchModelType && !searchAssetType)
                                                        )
                                                    )
                                       ).OrderBy(a => a.Name);

            CurrentIndex = index ?? 0;
            NextIndex = CurrentIndex + 8;
            PreviousIndex = CurrentIndex - 8;

            SearchCount = await queryAssets
                                .CountAsync();

            Assets = await queryAssets
                            .Skip(CurrentIndex)
                            .Take(8)
                            .ToArrayAsync();
            AssetCount = Assets.Length;

            string name = Request.Query["scName"];
            string description = Request.Query["scDesc"];

            // when name isn't defined, return the page
            // if it is defined, attempt to add the aggregator
            if (String.IsNullOrEmpty(name))
            {
                return Page();
            }

            // loop over each asset query
            // and add valid ones to the assets list
            var assets = new List<SporeServerAsset>();
            foreach (var assetString in Request.Query["asset"])
            {
                if (Int64.TryParse(assetString, out Int64 assetId))
                {
                    var asset = await _assetManager.FindByIdAsync(assetId, true);
                    if (asset != null)
                    {
                        assets.Add(asset);
                    }
                }
            }

            // make sure the aggregator has assets
            if (assets.Count == 0)
            {
                return Page();
            }

            // add aggregator
            var author = await _userManager.GetUserAsync(User);
            var aggregator = await _aggregatorManager.AddAsync(author, name, description, assets.ToArray());
            if (aggregator == null)
            {
                return StatusCode(500);
            }

            // redirect to /pollinator/atom/aggregator/{id}
            return Redirect($"https://pollinator.spore.com/pollinator/atom/aggregator/{aggregator.AggregatorId}");
        }
    }
}
