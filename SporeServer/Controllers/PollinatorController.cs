/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporeServer.Areas.Identity.Data;
using SporeServer.Controllers.Pollinator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using AtomFeed = SporeServer.AtomFeed.AtomFeedBuilder;
using SporeServer.AtomFeed;
using SporeServer.AtomFeed.Templates.Pollinator;

namespace SporeServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollinatorController : Controller
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly SignInManager<SporeServerUser> _signInManager;
        public PollinatorController(UserManager<SporeServerUser> userManager, SignInManager<SporeServerUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("handshake")]
        public async Task<IActionResult> HandShake()
        {
            string email, password;

            if (!HttpContext.Request.Headers.ContainsKey("Spore-User") ||
                String.IsNullOrEmpty(email = HttpContext.Request.Headers["Spore-User"]) ||
                !HttpContext.Request.Headers.ContainsKey("Spore-Password") ||
                String.IsNullOrEmpty((password = HttpContext.Request.Headers["Spore-Password"])))
            {
                return Unauthorized();
            }

            // TODO, use proper html decoder
            email = email.Replace("%40", "@");

            SporeServerUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result != SignInResult.Success)
            {
                return Unauthorized();
            }

            HandshakeTemplate template = new HandshakeTemplate(user);

            return AtomFeedBuilder.CreateFromTemplate<HandshakeTemplate>(template).ToContentResult();
        }

        [AllowAnonymous]
        [HttpPost("telemetry")]
        public async Task<IActionResult> Telemetry()
        {
            Console.WriteLine("/pollinator/telemetry");

            Console.WriteLine(String.Join(' ', Request.Headers["Content-Length"]));
            Console.WriteLine(String.Join(' ', Request.Headers["Content-Type"]));

            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                var buffer = new byte[512];
                var bytesRead = default(int);
                while ((bytesRead = (await Request.Body.ReadAsync(buffer, 0, buffer.Length))) > 0)
                    memstream.Write(buffer, 0, bytesRead);

                Console.WriteLine("stream length: " + memstream.Length);
                System.IO.File.WriteAllBytes("tmp.bin", memstream.ToArray());
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("event/upload")]
        public async Task<IActionResult> Upload()
        {
            Console.WriteLine("/pollinator/event/upload");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("public-interface/AssetUploadServlet")]
        public async Task<IActionResult> AssetUpload()
        {
            Console.WriteLine("/public-interface/AssetUploadServlet");

            Console.WriteLine(String.Join(' ', Request.Headers["Content-Length"]));
            Console.WriteLine(String.Join(' ', Request.Headers["Content-Type"]));

            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                var buffer = new byte[512];
                var bytesRead = default(int);
                while ((bytesRead = (await Request.Body.ReadAsync(buffer, 0, buffer.Length))) > 0)
                    memstream.Write(buffer, 0, bytesRead);

                Console.WriteLine("stream length: " + memstream.Length);
                System.IO.File.WriteAllBytes("tmp2.bin", memstream.ToArray());
            }

            return Ok();
        }
    }
}
