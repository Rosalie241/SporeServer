using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAsset
    {
        /// <summary>
        ///     The primary key
        /// </summary>
        [Key]
        public Int64 AssetId { get; set; }
        
        /// <summary>
        ///     Parent Asset Id
        /// </summary>
        public Int64 ParentAssetId { get; set; }

        /// <summary>
        ///     Whether it's used
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        ///     Asset Timestamp
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        ///     Asset Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }

        /// <summary>
        ///     Asset Author
        /// </summary>
        public SporeServerUser Author { get; set; }

        /// <summary>
        ///     Name of Asset
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     Description of Asset
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Tags of Asset
        /// </summary>
        public string Tags { get; set; }
        
        /// <summary>
        ///     FileSize of xml
        /// </summary>
        public Int64 Size { get; set; }

        /// <summary>
        ///     Automatically uploaded or not
        /// </summary>
        public bool Slurped { get; set; }
    }
}
