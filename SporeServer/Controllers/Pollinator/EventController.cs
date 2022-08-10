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
using SporeServer.Services;
using System;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IEventManager _eventManager;

        public EventController(UserManager<SporeServerUser> userManager, IEventManager eventManager)
        {
            _userManager = userManager;
            _eventManager = eventManager;
        }


        // POST /pollinator/event/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            Console.WriteLine($"/pollinator/event/upload{Request.QueryString}");

            var author = await _userManager.GetUserAsync(User);

            await _eventManager.AddAsync(Request.Body, author);
            return Ok();
        }
    }
}
