using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SporeServer.Controllers.Pollinator
{
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class AtomController : ControllerBase
    {

        [Authorize]
        [HttpGet("asset")]
        public async Task<IActionResult> Asset()
        {
           

            return Ok();
        }

    }
}
