using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAssetTrait
    {
        /// <summary>
        ///     Traid Id
        /// </summary>
        [Key]
        public Int64 TraitId { get; set; }
        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Asset
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Trait Type
        /// </summary>
        public SporeAssetTraitType TraitType { get; set; }
    }
}
