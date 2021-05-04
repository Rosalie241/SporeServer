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
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Builder.AtomFeed;
using SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom;
using SporeServer.Data;
using SporeServer.Services;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class AtomController : ControllerBase
    {
        private readonly IAssetManager _assetManager;
        private readonly UserManager<SporeServerUser> _userManager;
        public AtomController(IAssetManager assetManager, UserManager<SporeServerUser> userManager)
        {
            _assetManager = assetManager;
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

        // GET /pollinator/atom/subscribe
        [HttpGet("subscribe")]
        public IActionResult Subscribe()
        {
            Console.WriteLine($"/pollinator/atom/subscribe{Request.QueryString}");
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
