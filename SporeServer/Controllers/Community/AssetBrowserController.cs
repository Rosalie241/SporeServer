using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Community
{
    [Authorize]
    [Route("Community/[controller]")]
    [ApiController]
    public class AssetBrowserController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public AssetBrowserController(SporeServerContext context, UserManager<SporeServerUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // GET /community/assetBrowser/deleteAsset/{Id}
        [HttpGet("deleteAsset/{id}")]
        public async Task<IActionResult> DeleteAsset(Int64 Id)
        {
            var asset = await _context.Assets.FindAsync(Id);
            var user = await _userManager.GetUserAsync(User);

            // make sure the asset authorId
            // is the current user id
            if (asset.AuthorId != user.Id)
            {
                return Ok();
            }

            // remove the asset from the database
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            // attempt to cleanup asset files
            string baseFilePath = Path.Combine(_env.WebRootPath, "static", "usercontent");
            string xmlFile = Path.Combine(baseFilePath, $"{asset.AssetId}.xml");
            string pngFile = Path.Combine(baseFilePath, $"{asset.AssetId}.png");

            try
            {
                foreach (string file in new string[] { xmlFile, pngFile })
                {
                    System.IO.File.Delete(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("/community/assetBrowser/deleteAsset/" + Id);

            return Ok();
        }
    }
}
