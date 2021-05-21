using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAssetTag
    {
        /// <summary>
        ///     Tag Id
        /// </summary>
        [Key]
        public Int64 TagId { get; set; }
        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Asset
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Tag Value
        /// </summary>
        public string Tag { get; set; }
    }
}
