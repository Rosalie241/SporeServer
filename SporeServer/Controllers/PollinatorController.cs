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
using SporeServer.AtomFeed;
using SporeServer.AtomFeed.Templates.Pollinator;
using SporeServer.Data;

namespace SporeServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PollinatorController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;

        public PollinatorController(SporeServerContext context, UserManager<SporeServerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET /pollinator/handshake
        [HttpGet("handshake")]
        public async Task<IActionResult> HandShake()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.NextAssetId == 0)
            {
                // reserve new asset for user
                var asset = new SporeServerAsset()
                {
                    Used = false,
                    Author = user
                };

                await _context.Assets.AddAsync(asset);
                await _context.SaveChangesAsync();

                // update user
                user.NextAssetId = asset.AssetId;
                await _userManager.UpdateAsync(user);
            }

            return AtomFeedBuilder.CreateFromTemplate<HandshakeTemplate>(
                    new HandshakeTemplate(user)
                ).ToContentResult();
        }

        // POST /pollinator/telemetry
        [HttpPost("telemetry")]
        public IActionResult Telemetry()
        {
            Console.WriteLine("/pollinator/telemetry");
            return Ok();
        }

    }
}
