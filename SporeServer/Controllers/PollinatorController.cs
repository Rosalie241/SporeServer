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
using System;
using System.Threading.Tasks;
using SporeServer.Builder.AtomFeed;
using SporeServer.Builder.AtomFeed.Templates.Pollinator;
using SporeServer.Data;
using SporeServer.Services;
using System.Collections.Generic;

namespace SporeServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PollinatorController : ControllerBase
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAggregatorManager _aggregatorManager;
        private readonly IAggregatorSubscriptionManager _aggregatorSubscriptionManager;

        public PollinatorController(UserManager<SporeServerUser> userManager, IAssetManager assetManager, IUserSubscriptionManager userSubscriptionManager, IAggregatorManager aggregatorManager, IAggregatorSubscriptionManager aggregatorSubscriptionManager)
        {
            _userManager = userManager;
            _assetManager = assetManager;
            _userSubscriptionManager = userSubscriptionManager;
            _aggregatorManager = aggregatorManager;
            _aggregatorSubscriptionManager = aggregatorSubscriptionManager;
        }

        // GET /pollinator/handshake
        [HttpGet("handshake")]
        public async Task<IActionResult> HandShake()
        {
            var user = await _userManager.GetUserAsync(User);
            var asset = await _assetManager.FindByIdAsync(user.NextAssetId);
            var aggregators = await _aggregatorManager.FindByAuthorAsync(user);
            var userSubscriptions = await _userSubscriptionManager.FindAllByAuthorAsync(user);
            var aggregatorSubscriptions = await _aggregatorSubscriptionManager.FindAllByAuthorAsync(user);
            var aggregatorSubscriptionCounts = new List<Int32>();

            // retrieve subscription count for each aggregator
            foreach (var aggregator in aggregators)
            {
                var aggregatorSubscriptionCount = await _aggregatorSubscriptionManager.GetSubscriberCountAsync(aggregator);
                aggregatorSubscriptionCounts.Add(aggregatorSubscriptionCount);
            }

            // reserve new asset when 
            //      * user has no reserved asset
            //      * user has a used asset
            if (user.NextAssetId == 0 ||
                (asset != null && asset.Used))
            {
                // when reserving a new asset fails, error out
                if (!await _assetManager.ReserveAsync(user))
                {
                    return StatusCode(500);
                }
            }

            return AtomFeedBuilder.CreateFromTemplate(
                    new HandshakeTemplate(user, aggregators, aggregatorSubscriptionCounts.ToArray(), userSubscriptions, aggregatorSubscriptions)
                ).ToContentResult();
        }

        // POST /pollinator/telemetry
        [HttpPost("telemetry")]
        public IActionResult Telemetry()
        {
            return Ok();
        }
    }
}
