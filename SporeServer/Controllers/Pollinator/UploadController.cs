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
using SporeServer.Builder.Xml;
using SporeServer.Builder.Xml.Templates.Pollinator.Upload;
using SporeServer.Data;
using SporeServer.Services;
using System;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;

        public UploadController(SporeServerContext context, UserManager<SporeServerUser> userManager, IAssetManager assetManager)
        {
            _context = context;
            _userManager = userManager;
            _assetManager = assetManager;
        }

        // GET /pollinator/upload/status/{id}
        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(Int64 id)
        {
            Console.WriteLine($"/pollinator/Upload/Status/{id}");

            Int64 nextId = id;

            var user = await _userManager.GetUserAsync(User);
            var asset = await _assetManager.FindByIdAsync(id);

            // we're only successful when
            //  * the asset exists
            //  * the asset is used
            //  * the asset has the same author
            //    as the currently logged in user
            //  * the id is the same as the user's reserved id
            bool success = (asset != null
                && asset.Used
                && asset.AuthorId == user.Id
                && user.NextAssetId == id);
            
            // when the requirements have been met,
            // reserve a new asset
            if (success)
            {
                // when reserving a new asset fails, error out
                if (!await _assetManager.ReserveAsync(user))
                {
                    return StatusCode(500);
                }

                // tell the client about the new id
                nextId = user.NextAssetId;
            }

            return XmlBuilder.CreateFromTemplate(
                    new StatusTemplate(nextId, success)
                ).ToContentResult();
        }

    }
}
