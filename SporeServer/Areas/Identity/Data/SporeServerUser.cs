using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SporeServer.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SporeServerUser class
    public class SporeServerUser : IdentityUser<Int64>
    {
        /// <summary>
        ///     Reserved Next Asset Id
        /// </summary>
        public Int64 NextAssetId { get; set; }

    }
}
