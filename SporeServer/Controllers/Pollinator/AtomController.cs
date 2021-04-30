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
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Builder.AtomFeed;
using SporeServer.Builder.AtomFeed.Templates.Pollinator.Atom;
using SporeServer.Data;
using SporeServer.Services;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace SporeServer.Controllers.Pollinator
{
    [Authorize]
    [Route("Pollinator/[controller]")]
    [ApiController]
    public class AtomController : ControllerBase
    {
        private readonly SporeServerContext _context;
        private readonly IAssetManager _assetManager;
        public AtomController(SporeServerContext context, IAssetManager assetManager)
        {
            _context = context;
            _assetManager = assetManager;
        }

        // GET /pollinator/atom/randomAsset
        [HttpGet("randomAsset")]
        public IActionResult RandomAsset()
        {
            Console.WriteLine($"/pollinator/atom/randomAsset{Request.QueryString}");

            string modelType = Request.Query["asset.function"];

            if (!Enum.IsDefined(typeof(SporeModelType), Int64.Parse(modelType)))
            {
                Console.WriteLine("UNDEFINED: " + modelType);
            }
            else
            {
                SporeModelType type = (SporeModelType)Int64.Parse(modelType);

                Console.WriteLine("DEFINED: " + type.ToString());
            }

            return Ok();
        }

        // GET /pollinator/atom/downloadQueue
        [HttpGet("downloadQueue")]
        public IActionResult DownloadQueue()
        {
            Console.WriteLine($"/pollinator/atom/downloadQueue{Request.QueryString}");


            string date = XmlConvert.ToString(DateTime.Now, XmlDateTimeSerializationMode.Utc);
            /*
            return new ContentResult()
            {
                Content = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<feed xmlns=""http://www.w3.org/2005/Atom"" xmlns:sp=""http://spore.com/atom"" xml:lang=""en_US"">
   <id>tag:spore.com,2006:downloadQueue</id>
   <title>Rosalie</title>
   <updated>{date}</updated>
   <author>
      <name>Rosalie</name>
      <uri>13</uri>
   </author>
   <subcount>0</subcount>
   <link rel=""self"" href=""https://pollinator.spore.com/pollinator/atom/downloadQueue"" />
   <entry>
      <id>tag:spore.com,2006:asset/501073471139</id>
      <title>Scythovatid</title>
      <link rel=""alternate"" href=""https://pollinator.spore.com/pollinator/sporepedia#qry=sast-501073471139"" />
      <updated>{date}</updated>
      <author>
         <name>TheAWKLORD</name>
         <uri>501071978458</uri>
      </author>
      <modeltype>0x9ea3031a</modeltype>
      <locale>en_US</locale>
      <modeltype>0x9ea3031a</modeltype>
      <sp:ownership>
         <sp:original name=""id"" type=""int"">501073471139</sp:original>
         <sp:parent name=""id"" type=""int"">0</sp:parent>
      </sp:ownership>
      <content type=""html"">
         <img src=""https://static.spore.com/tmp/501073471139.png3"" />
      </content>
      <link rel=""enclosure"" href=""https://static.spore.com/tmp/501073471139.png"" type=""image/png"" length=""80483"" />
      <link rel=""enclosure"" href=""https://static.spore.com/tmp/501073471139.xml"" type=""application/x-creature+xml"" />
      <summary>Following the discovery of rare underground spice geysers in 4780 C.S.E. was a series of reunification wars led by the Vaan'kthulvid dynasty in their efforts to monopolize this highly lucrative and powerful spice variant.</summary>
   </entry>
</feed>",
                ContentType = "application/atom+xml"
            };*/
            return Ok();
        }

        // GET /pollinator/atom/asset/{id?}
        [HttpGet("asset/{id?}")]
        public async Task<IActionResult> Asset(Int64? id)
        {
            Console.WriteLine($"/pollinator/atom/asset/{id}{Request.QueryString}");

            var assets = new List<SporeServerAsset>();

            if (id != null)
            {
                // parameter is asset id
                var asset = await _assetManager.FindByIdAsync((Int64)id, true);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            else
            {
                // loop over all id queries
                foreach (var idQuery in Request.Query["id"])
                {
                    // make sure we can parse the id
                    // and that the asset exists
                    if (Int64.TryParse(idQuery, out Int64 assetId))
                    {
                        var asset = await _assetManager.FindByIdAsync(assetId, true);
                        if (asset != null)
                        {
                            assets.Add(asset);
                        }
                    }
                }
            }

            Console.WriteLine(AtomFeedBuilder.CreateFromTemplate(
                    new AssetTemplate(assets.ToArray())
                ).ToContentResult().Content);

            return AtomFeedBuilder.CreateFromTemplate(
                    new AssetTemplate(assets.ToArray())
                ).ToContentResult();
       
        }

        // GET /pollinator/atom/subscribe
        [HttpGet("subscribe")]
        public IActionResult Subscribe()
        {
            Console.WriteLine($"/pollinator/atom/subscribe{Request.QueryString}");

            foreach (var a in Request.Query)
            {
                Console.WriteLine($"{a.Key}: {String.Join(' ', a.Value)}");
            }

            return Ok();
        }

        // GET /pollinator/atom/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult AtomUser(Int64 userId)
        {
            Console.WriteLine("/pollinator/atom/user/" + userId);
            return Ok();
        }

    }
}
