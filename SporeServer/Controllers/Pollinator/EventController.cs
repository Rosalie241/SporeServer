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
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        // POST /pollinator/event/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            Console.WriteLine($"/pollinator/event/upload{Request.QueryString}");

            using (var stream = System.IO.File.Open("event.txt", FileMode.Create))
            {
                await Request.Body.CopyToAsync(stream);
            }
            return Ok();
        }
    }
}
