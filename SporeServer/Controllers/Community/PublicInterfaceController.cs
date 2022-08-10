/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using SporeServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SporeServer.Controllers.Community
{
    [Authorize]
    [Route("Community/public-interface")]
    [ApiController]
    public class PublicInterfaceController : ControllerBase
    {
        // POST /community/public-interface/SnapshotUploadServlet
        [HttpPost("SnapshotUploadServlet")]
        public IActionResult SnapshotUploadServlet([FromForm] SnapshotUploadForm file)
        {
            // TODO
            return Ok();
        }
    }
}
