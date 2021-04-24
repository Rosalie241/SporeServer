using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;

        public UploadController(SporeServerContext context, UserManager<SporeServerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET /pollinator/upload/status/{id}
        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(Int64 id)
        {
            Int64 nextId = id;

            var user = await _userManager.GetUserAsync(User);
            var asset = await _context.Assets.FindAsync(id);

            // we're only successful when
            //  * the asset exists
            //  * the asset is used
            //  * the asset has the same author
            //    as the currently logged in user
            //  * the id is the same as the user's reserved id
            bool success = (asset != null
                && asset.Used
                && asset.AuthorId == user.Id
                && user.NextAssetId == id);
            
            // when the requirements have been met,
            // reserve a new asset
            if (success)
            {
                var newAsset = new SporeServerAsset()
                {
                    Used = false,
                    Author = user
                };

                await _context.Assets.AddAsync(newAsset);
                await _context.SaveChangesAsync();

                // update user
                user.NextAssetId = newAsset.AssetId;
                await _userManager.UpdateAsync(user);

                // tell the client about it
                nextId = newAsset.AssetId;
            }

            Console.WriteLine("/Pollinator/Upload/Status/" + id);

            // TODO, use serialization for this
            // Success
            // Failed
            return new ContentResult()
            {
                ContentType = "text/xml",
                Content = $@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><PollinatorResponse><next-id>{nextId}</next-id><Status>{(success ? "Success" : "Failed")}</Status></PollinatorResponse>"
            };
        }

    }
}
