using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Models
{
    public class AssetUploadForm
    {
        /// <summary>
        ///     Asset Type Id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }

        //public int[] traitguids { get; set; }
        
        /// <summary>
        ///     Asset Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Asset Tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        ///     Asset Xml,
        ///     filename is asset name
        /// </summary>
        public IFormFile ModelData { get; set; }
        
        /// <summary>
        ///     Asset PNG
        /// </summary>
        public IFormFile ThumbnailData { get; set; }
    }
}
