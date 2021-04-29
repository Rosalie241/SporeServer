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
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;
        public AssetBrowserController(SporeServerContext context, UserManager<SporeServerUser> userManager, IAssetManager assetManager)
        {
            _context = context;
            _userManager = userManager;
            _assetManager = assetManager;
        }

        // GET /community/assetBrowser/deleteAsset/{id}
        [HttpGet("deleteAsset/{id}")]
        public async Task<IActionResult> DeleteAsset(Int64 id)
        {
            Console.WriteLine($"/community/assetBrowser/deleteAsset/{id}{Request.QueryString}");

            var asset = await _context.Assets.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            // make sure the asset exists
            if (asset == null)
            {
                return Ok();
            }

            // make sure the asset authorId
            // is the current user id
            if (asset.AuthorId != user.Id)
            {
                return Ok();
            }

            // delete the asset
            await _assetManager.DeleteAsync(asset);

            return Ok();
        }
    }
}
