using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Models;
using SporeServer.Models.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/public-interface")]
    [ApiController]
    public class PublicInterfaceController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public PublicInterfaceController(SporeServerContext context, UserManager<SporeServerUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // POST /pollinator/public-interface/AssetUploadServlet
        [HttpPost("AssetUploadServlet")]
        public async Task<IActionResult> AssetUploadServlet([FromForm] AssetUploadForm formAsset)
        {
            Console.WriteLine($"Pollinator/public-interface/AssetUploadServlet{Request.QueryString}");

            int slurpValue = 0;

            // the game client always sends the slurp query
            // and it's always either 0 or 1
            if (!Request.Query.ContainsKey("slurp") ||
                !int.TryParse(Request.Query["slurp"], out slurpValue) ||
                (slurpValue != 0 && slurpValue != 1))
            {
                return Ok();
            }

            Int64 parentId = 0;
            SporeServerAsset parentAsset = null;

            // the game sometimes sends a parent id,
            // make sure we can parse it
            if (Request.Query.ContainsKey("parent") &&
                !Int64.TryParse(Request.Query["parent"], out parentId))
            {
                return Ok();
            }

            // try to find the parent asset
            if (parentId != 0)
            {
                parentAsset = await _context.Assets.FindAsync(parentId);
            }

            var user = await _userManager.GetUserAsync(User);

            // make sure the requested assetId is the user's nextAssetId
            if (user.NextAssetId != formAsset.AssetId)
            {
                return Ok();
            }

            var asset = await _context.Assets.FindAsync(formAsset.AssetId);

            // make sure the asset exists and
            // make sure it isn't already used
            if (asset == null ||
                asset.Used)
            {
                return Ok();
            }

            string baseFilePath = Path.Combine(_env.WebRootPath, "static", "usercontent");
            string xmlFile = Path.Combine(baseFilePath, $"{asset.AssetId}.xml");
            string pngFile = Path.Combine(baseFilePath, $"{asset.AssetId}.png");

            try
            {
                // make sure the directory exists
                if (!Directory.Exists(baseFilePath))
                {
                    Directory.CreateDirectory(baseFilePath);
                }

                // make sure we can load the xml
                // and validate it
                SporeModel model = SporeModel.SerializeFromXml(formAsset.ModelData.OpenReadStream());
                SporeModel.Validate(model);

                // save PNG & XML
                System.IO.File.WriteAllText(xmlFile, SporeModel.DeserializeToxml(model));
                using (Stream stream = System.IO.File.OpenWrite(pngFile))
                {
                    await formAsset.ThumbnailData.CopyToAsync(stream);
                    await stream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                // cleanup when failed
                foreach (string file in new string[] { xmlFile, pngFile })
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }

                Console.WriteLine(e);
                return Ok();
            }

            // update the previously unused asset
            asset.Used = true;
            asset.Timestamp = DateTime.Now;
            asset.ParentAssetId = parentId;
            asset.Name = formAsset.ModelData.FileName;
            asset.Description = formAsset.Description;
            asset.Size = formAsset.ModelData.Length;
            asset.Slurped = slurpValue == 1;
            _context.Assets.Update(asset);

            // save the database
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("SnapshotUploadServlet")]
        public async Task<IActionResult> SnapshotUploadServlet()
        {
            Console.WriteLine("Pollinator/public-interface/SnapshotUploadServlet");

            var file = System.IO.File.OpenWrite("postcard.txt");

            await Request.Body.CopyToAsync(file);

            file.Flush();
            file.Close();

            return Ok();
        }
    }
}
