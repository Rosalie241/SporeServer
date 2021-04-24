using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Community
{
    public class TestFile2
    {
        public string Locale { get; set; }
        public string MyEmail { get; set; }
        public string DestEmail { get; set; }
        public string MsgText { get; set; }
        public IFormFile Imagedata { get; set; }
    }

    [Authorize]
    [Route("Community/public-interface")]
    [ApiController]
    public class PublicInterfaceController : ControllerBase
    {
        // POST /community/public-interface/SnapshotUploadServlet

        [HttpPost("SnapshotUploadServlet")]
        public async Task<IActionResult> SnapshotUploadServlet([FromForm] TestFile2 file)
        {
            // TODO, hook this up to an email server

            
            Console.WriteLine("/community/public-interface/SnapshotUploadServlet");

            Console.WriteLine(file.DestEmail);

            Console.WriteLine(file.Imagedata.Length);

            await file.Imagedata.CopyToAsync(System.IO.File.OpenWrite("postcard2.png"));

            /*var file2 = System.IO.File.OpenWrite("postcard.txt");

            await Request.Body.CopyToAsync(file);

            file.Flush();
            file.Close();*/

            return Ok();
        }
    }
}
