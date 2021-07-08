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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Services;
using SporeServer.SporeTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Community
{
    [Authorize]
    [Route("Community/[controller]")]
    [ApiController]
    public class AssetBrowserController : ControllerBase
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;
        private readonly IAggregatorManager _aggregatorManager;
        private readonly IRatingManager _ratingManager;

        public AssetBrowserController(UserManager<SporeServerUser> userManager, IAssetManager assetManager, IAggregatorManager aggregatorManager, IRatingManager ratingManager)
        {
            _userManager = userManager;
            _assetManager = assetManager;
            _aggregatorManager = aggregatorManager;
            _ratingManager = ratingManager;
        }

        // GET /community/assetBrowser/deleteAsset/{id}
        [HttpGet("deleteAsset/{id}")]
        public async Task<IActionResult> DeleteAsset(Int64 id)
        {
            Console.WriteLine($"/community/assetBrowser/deleteAsset/{id}{Request.QueryString}");

            var asset = await _assetManager.FindByIdAsync(id);
            var user = await _userManager.GetUserAsync(User);

            // make sure the asset exists
            if (asset == null ||
                asset.Used)
            {
                return Ok();
            }

            // make sure the asset author
            // is the current user
            if (asset.AuthorId == user.Id)
            {
                if (!await _assetManager.DeleteAsync(asset))
                {
                    return StatusCode(500);
                }
            }

            return Ok();
        }

        /// <summary>
        ///     Simple helper function for editSporecast which tries to get SporeServerAggregator from idQuery, returns null when not found
        /// </summary>
        /// <param name="uriQuery"></param>
        /// <returns></returns>
        private async Task<SporeServerAggregator> GetAggregatorFromIdQuery(string idQuery)
        {
            // make sure the uri request starts with
            // the correct tag
            if (!idQuery.StartsWith("tag:spore.com,2006:aggregator/"))
            {
                return null;
            }

            string uriAggregator = idQuery.Remove(0, 30);

            // make sure we can parse the aggregator id
            if (!Int64.TryParse(uriAggregator, out Int64 aggregatorId))
            {
                return null;
            }

            return await _aggregatorManager.FindByIdAsync(aggregatorId);
        }

        // GET /community/assetBrowser/editSporecast
        [HttpGet("editSporecast")]
        public async Task<IActionResult> EditSporecast()
        {
            Console.WriteLine($"/community/assetBrowser/editSporecast{Request.QueryString}");

            var aggregator = await GetAggregatorFromIdQuery(Request.Query["scId"]);
            if (aggregator == null)
            {
                return NotFound();
            }

            aggregator.Description = Request.Query["scDesc"];

            // add requested assets
            foreach (var addQuery in Request.Query["scAdd"])
            {
                if (Int64.TryParse(addQuery, out Int64 assetId))
                {
                    var asset = await _assetManager.FindByIdAsync(assetId);
                    // only add asset when it exists
                    if (asset != null)
                    {
                        aggregator.Assets.Add(asset);
                    }
                }
            }

            // remove requested assets
            foreach (var removeQuery in Request.Query["scRemove"])
            {
                if (Int64.TryParse(removeQuery, out Int64 assetId))
                {
                    var asset = await _assetManager.FindByIdAsync(assetId);
                    // only remove asset when it exists
                    // and is in the aggregator
                    if (asset != null &&
                        aggregator.Assets.Contains(asset))
                    {
                        aggregator.Assets.Remove(asset);
                    }
                }
            }

            // update aggregator
            if (!await _aggregatorManager.UpdateAsync(aggregator))
            {
                return StatusCode(500);
            }

            // redirect to /pollinator/atom/aggregator/{id}
            return Redirect($"https://pollinator.spore.com/pollinator/atom/aggregator/{aggregator.AggregatorId}");
        }

        // GET /community/assetBrowser/rate
        [HttpGet("rate")]
        public async Task<IActionResult> Rate()
        {
            Console.WriteLine($"/community/assetBrowser/rate{Request.QueryString}");

            // make sure the rating query exists, and is either 0 or 1
            if (Int32.TryParse(Request.Query["rating"], out Int32 ratingNumber))
            {
                if (ratingNumber != 0 &&
                    ratingNumber != 1)
                {
                    return Ok();
                }
            }

            bool rating = ratingNumber == 1;

            // make sure we can parse the assetId query
            if (Int64.TryParse(Request.Query["assetId"], out Int64 assetId))
            {
                var author = await _userManager.GetUserAsync(User);
                var asset = await _assetManager.FindByIdAsync(assetId);

                // make sure the asset exists
                if (asset != null &&
                    asset.Used)
                {
                    // check if the rating already exists
                    var assetRating = await _ratingManager.FindAsync(author, asset);
                    if (assetRating != null)
                    { // already exists, so update rating
                        // only update rating when rating differs
                        if (assetRating.Rating != rating)
                        {
                            assetRating.Rating = rating;

                            // when updating fails, error out
                            if (!await _ratingManager.UpdateAsync(assetRating))
                            {
                                return StatusCode(500);
                            }
                        }
                    }
                    else
                    { // doesn't exist, so add rating
                        // when adding the rating fails, error out
                        if (!await _ratingManager.AddAsync(author, asset, rating))
                        {
                            return StatusCode(500);
                        }
                    }
                }
            }

            return Ok();
        }
    }
}
