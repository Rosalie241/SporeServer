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
        private readonly IAggregatorManager _aggregatorManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAggregatorSubscriptionManager _aggregatorSubscriptionManager;
        private readonly UserManager<SporeServerUser> _userManager;

        public AtomController(IAssetManager assetManager, IAggregatorManager aggregatorManager, 
                                IUserSubscriptionManager userSubscriptionManager, 
                                IAggregatorSubscriptionManager aggregatorSubscriptionManager,  
                                UserManager<SporeServerUser> userManager)
        {
            _assetManager = assetManager;
            _aggregatorManager = aggregatorManager;
            _userSubscriptionManager = userSubscriptionManager;
            _aggregatorSubscriptionManager = aggregatorSubscriptionManager;
            _userManager = userManager;
        }

        // GET /pollinator/atom/randomAsset
        [HttpGet("randomAsset")]
        public async Task<IActionResult> RandomAsset()
        {
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
            var user = await _userManager.GetUserAsync(User);

            return AtomFeedBuilder.CreateFromTemplate(
                    new DownloadQueueTemplate(user, null)
                ).ToContentResult();
        }

        // GET /pollinator/atom/asset/{id?}
        [HttpGet("asset/{id?}")]
        public async Task<IActionResult> Asset(Int64? id)
        {
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

            return AtomFeedBuilder.CreateFromTemplate(
                    new AssetTemplate(assets.ToArray())
                ).ToContentResult();
       
        }

        // GET /pollinator/atom/subscribe
        [HttpGet("subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            string uriQuery = Request.Query["uri"];

            var user = await ControllerHelper.GetUserFromQuery(_userManager, uriQuery);
            var aggregator = await ControllerHelper.GetAggregatorFromQuery(_aggregatorManager, uriQuery);

            if (user == null && aggregator == null)
            {
                return Ok();
            }

            var author = await _userManager.GetUserAsync(User);

            if (user != null)
            { // user subscription 
                // only add subscription when the subscription doesn't already exist
                // and if the requested user is not the author
                if (await _userSubscriptionManager.FindAsync(author, user) == null &&
                    user.Id != author.Id)
                {
                    // add subscription
                    if (!await _userSubscriptionManager.AddAsync(author, user))
                    {
                        return StatusCode(500);
                    }
                }

                // redirect request to /user/{userId}
                return await AtomUser(user.Id);
            }
            else
            { // aggregator subscription
                // only add subscription when the subscription doesn't already exist
                // and if the requested aggregator author is not the author
                if (await _aggregatorSubscriptionManager.FindAsync(author, aggregator) == null &&
                    author.Id != aggregator.AuthorId)
                {
                    // add subscription
                    if (!await _aggregatorSubscriptionManager.AddAsync(author, aggregator))
                    {
                        return StatusCode(500);
                    }
                }

                // redirect request to /aggregator/{id}
                return await Aggregator(aggregator.AggregatorId);
            }
        }

        // GET /pollinator/atom/unsubscribe
        [HttpGet("unsubscribe")]
        public async Task<IActionResult> Unsubscribe()
        {
            string uriQuery = Request.Query["uri"];

            var user = await ControllerHelper.GetUserFromQuery(_userManager, uriQuery);
            var aggregator = await ControllerHelper.GetAggregatorFromQuery(_aggregatorManager, uriQuery);

            if (user == null && aggregator == null)
            {
                return Ok();
            }

            var author = await _userManager.GetUserAsync(User);

            if (user != null)
            { // user subscription
                var subscription = await _userSubscriptionManager.FindAsync(author, user);

                // only remove subscription when it exists
                if (subscription != null)
                {
                    // remove subscription
                    if (!await _userSubscriptionManager.RemoveAsync(subscription))
                    {
                        return StatusCode(500);
                    }
                }
            }
            else
            { // aggregator subscription
                var subscription = await _aggregatorSubscriptionManager.FindAsync(author, aggregator);

                // only remove subscription when it exists
                if (subscription != null)
                {
                    // remove subscription
                    if (!await _aggregatorSubscriptionManager.RemoveAsync(subscription))
                    {
                        return StatusCode(500);
                    }
                }
            }

            return Ok();
        }

        // GET /pollinator/atom/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> AtomUser(Int64 userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            // make sure the user exists
            if (user == null)
            {
                return NotFound();
            }

            var assets = await _assetManager.FindAllByUserIdAsync(userId);

            return AtomFeedBuilder.CreateFromTemplate(
                    new UserTemplate(user, assets)
                ).ToContentResult();
        }

        // GET /pollinator/atom/aggregator/{id}
        [HttpGet("aggregator/{id}")]
        public async Task<IActionResult> Aggregator(Int64 id)
        {
            var aggregator = await _aggregatorManager.FindByIdAsync(id);

            // make sure the aggregator exists
            if (aggregator == null)
            {
                return NotFound();
            }

            var subscriberCount = await _aggregatorSubscriptionManager.GetSubscriberCountAsync(aggregator);

            return AtomFeedBuilder.CreateFromTemplate(
                    new AggregatorTemplate(aggregator, subscriberCount)
                ).ToContentResult();
        }

        // GET /pollinator/atom/delete
        [HttpGet("delete")]
        public async Task<IActionResult> Delete()
        {
            var aggregator = await ControllerHelper.GetAggregatorFromQuery(_aggregatorManager, Request.Query["uri"]);

            // make sure the aggregator exists
            if (aggregator == null)
            {
                return Ok();
            }

            var user = await _userManager.GetUserAsync(User);

            // only remove aggregator when current user is the author
            if (aggregator.AuthorId == user.Id)
            {
                await _aggregatorManager.RemoveAsync(aggregator);
            }

            return Ok();
        }
    }
}
