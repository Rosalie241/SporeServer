using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
