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
using SporeServer.Areas.Identity.Data;
using SporeServer.Builder.AtomFeed;
using SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom;
using SporeServer.Services;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class AtomController : ControllerBase
    {
        private readonly IAssetManager _assetManager;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly UserManager<SporeServerUser> _userManager;

        public AtomController(IAssetManager assetManager, ISubscriptionManager subscriptionManager, UserManager<SporeServerUser> userManager)
        {
            _assetManager = assetManager;
            _subscriptionManager = subscriptionManager;
            _userManager = userManager;
        }

        // GET /pollinator/atom/randomAsset
        [HttpGet("randomAsset")]
        public async Task<IActionResult> RandomAsset()
        {
            Console.WriteLine($"/pollinator/atom/randomAsset{Request.QueryString}");

            string functionString = Request.Query["asset.function"];

            SporeServerAsset[] assets = null;

            // make sure we can parse the function string
            if (Int64.TryParse(functionString, out Int64 function))
            {
                // we only support modeltypes for now,
                // TODO: support archetypes/herdtypes
                if (Enum.IsDefined(typeof(SporeModelType), function))
                {
                    var user = await _userManager.GetUserAsync(User);
                    assets = await _assetManager.GetRandomAssetsAsync(user.Id, (SporeModelType)function);
                }
                else
                {
                    Console.WriteLine($"unsupported randomAsset function: {function}");
                }
            }

            return AtomFeedBuilder.CreateFromTemplate(
                    new RandomAssetTemplate(assets)
                ).ToContentResult();
        }

        // GET /pollinator/atom/downloadQueue
        [HttpGet("downloadQueue")]
        public async Task<IActionResult> DownloadQueue()
        {
            Console.WriteLine($"/pollinator/atom/downloadQueue{Request.QueryString}");

            var user = await _userManager.GetUserAsync(User);

            return AtomFeedBuilder.CreateFromTemplate(
                    new DownloadQueueTemplate(user, null)
                ).ToContentResult();
        }

        // GET /pollinator/atom/asset/{id?}
        [HttpGet("asset/{id?}")]
        public async Task<IActionResult> Asset(Int64? id)
        {
            Console.WriteLine($"/pollinator/atom/asset/{id}{Request.QueryString}");

            var assets = new List<SporeServerAsset>();

            if (id != null)
            {
                // parameter is asset id
                var asset = await _assetManager.FindByIdAsync((Int64)id, true);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            else
            {
                // loop over all id queries
                foreach (var idQuery in Request.Query["id"])
                {
                    // make sure we can parse the id
                    // and that the asset exists
                    if (Int64.TryParse(idQuery, out Int64 assetId))
                    {
                        var asset = await _assetManager.FindByIdAsync(assetId, true);
                        if (asset != null)
                        {
                            assets.Add(asset);
                        }
                    }
                }
            }

            Console.WriteLine(AtomFeedBuilder.CreateFromTemplate(
                    new AssetTemplate(assets.ToArray())
                ).ToContentResult().Content);

            return AtomFeedBuilder.CreateFromTemplate(
                    new AssetTemplate(assets.ToArray())
                ).ToContentResult();
       
        }

        /// <summary>
        ///     Simple helper function for atom/(un)subscribe which tries to get SporeServerUser from uriQuery, returns null when not found
        /// </summary>
        /// <param name="uriQuery"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<SporeServerUser> GetUserFromUriQuery(string uriQuery)
        {
            // make sure the uri request contains
            // the correct tag
            if (!uriQuery.Contains("tag:spore.com,2006:user/"))
            {
                return null;
            }

            string uriUser = uriQuery.Remove(0, 24);

            // make sure we can parse the user id
            if (!Int64.TryParse(uriUser, out Int64 userId))
            {
                return null;
            }

            return await _userManager.FindByIdAsync($"{userId}");
        }

        // GET /pollinator/atom/subscribe
        [HttpGet("subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            Console.WriteLine($"/pollinator/atom/subscribe{Request.QueryString}");

            var user = await GetUserFromUriQuery(Request.Query["uri"]);
            if (user == null)
            {
                return Ok();
            }

            var author = await _userManager.GetUserAsync(User);

            // only add subscription when the subscription doesn't already exist
            if (_subscriptionManager.Find(author, user) == null)
            {
                // add subscription
                if (!await _subscriptionManager.AddAsync(author, user))
                {
                    return StatusCode(500);
                }
            }

            // redirect request to /user/{userId}
            return await AtomUser(user.Id);
        }

        // GET /pollinator/atom/unsubscribe
        [HttpGet("unsubscribe")]
        public async Task<IActionResult> Unsubscribe()
        {
            Console.WriteLine($"/pollinator/atom/unsubscribe{Request.QueryString}");

            var user = await GetUserFromUriQuery(Request.Query["uri"]);
            if (user == null)
            {
                return Ok();
            }

            var author = await _userManager.GetUserAsync(User);
            var subscription = _subscriptionManager.Find(author, user);

            // only remove subscription when it exists
            if (subscription != null)
            {
                // remove subscription
                if (!await _subscriptionManager.RemoveAsync(subscription))
                {
                    return StatusCode(500);
                }
            }

            return Ok();
        }

        // GET /pollinator/atom/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> AtomUser(Int64 userId)
        {
            Console.WriteLine($"/pollinator/atom/user/{userId}{Request.QueryString}");

            var user = await _userManager.FindByIdAsync(userId.ToString());

            // make sure the user exists
            if (user == null)
            {
                return Ok();
            }

            var assets = await _assetManager.FindAllByUserIdAsync(userId);

            Console.WriteLine(AtomFeedBuilder.CreateFromTemplate(
                    new UserTemplate(user, assets)
                ).ToContentResult().Content);

            return AtomFeedBuilder.CreateFromTemplate(
                    new UserTemplate(user, assets)
                ).ToContentResult();
        }
    }
}
