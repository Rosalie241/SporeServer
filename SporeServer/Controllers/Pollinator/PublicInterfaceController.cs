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
using SporeServer.Data;
using SporeServer.Models;
using SporeServer.Services;
using SporeServer.SporeTypes;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/public-interface")]
    [ApiController]
    public class PublicInterfaceController : ControllerBase
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;

        public PublicInterfaceController(UserManager<SporeServerUser> userManager, IAssetManager assetManager)
        {
            _userManager = userManager;
            _assetManager = assetManager;
        }

        // POST /pollinator/public-interface/AssetUploadServlet
        [HttpPost("AssetUploadServlet")]
        public async Task<IActionResult> AssetUploadServlet([FromForm] AssetUploadForm formAsset)
        {
            Console.WriteLine($"/pollinator/public-interface/AssetUploadServlet{Request.QueryString}");

            // the game client always sends the slurp query
            // and it's always either 0 or 1
            if (!Request.Query.ContainsKey("slurp") ||
                !int.TryParse(Request.Query["slurp"], out int slurpValue) ||
                (slurpValue != 0 && slurpValue != 1))
            {
                return Ok();
            }

            Int64 parentId = 0;

            // the game sometimes sends a parent id,
            // make sure we can parse it
            if (Request.Query.ContainsKey("parent") &&
                !Int64.TryParse(Request.Query["parent"], out parentId))
            {
                return Ok();
            }

            // when we can't find the asset,
            // reset parentId
            if (parentId != 0 &&
                (await _assetManager.FindByIdAsync(parentId)) == null)
            {
                parentId = 0;
            }

            // the game always sends the type id
            // make sure we can parse it
            // and that's it a valid id
            if (!Int64.TryParse(formAsset.TypeId.TrimStart('0', 'x'),
                    NumberStyles.HexNumber,
                    null,
                    out Int64 typeId) ||
                !Enum.IsDefined(typeof(SporeAssetType), typeId))
            {
                Console.WriteLine($"invalid type id: {typeId}");
                return Ok();
            }

            var user = await _userManager.GetUserAsync(User);

            // make sure the requested assetId is the user's nextAssetId
            if (user.NextAssetId != formAsset.AssetId)
            {
                return Ok();
            }

            var asset = await _assetManager.FindByIdAsync(formAsset.AssetId);

            // make sure the asset exists and
            // make sure it isn't already used
            if (asset == null ||
                asset.Used)
            {
                return Ok();
            }

            // make sure the asset doesn't go over any limits
            if ((formAsset.Description != null &&
                formAsset.Description.Length > 256) ||
                (formAsset.ModelData != null && 
                formAsset.ModelData.FileName.Length > 32) ||
                (formAsset.Tags != null && 
                formAsset.Tags.Length > 256))
            {
                return Ok();
            }

            // save the asset
            await _assetManager.AddAsync(formAsset, 
                asset,
                parentId,
                (slurpValue == 1),
                (SporeAssetType)typeId);

            return Ok();
        }

        [HttpPost("SnapshotUploadServlet")]
        public async Task<IActionResult> SnapshotUploadServlet()
        {
            Console.WriteLine("Pollinator/public-interface/SnapshotUploadServlet");

            var file = System.IO.File.OpenWrite("postcard.txt");

            await Request.Body.CopyToAsync(file);

            file.Flush();
            file.Close();

            return Ok();
        }
    }
}
