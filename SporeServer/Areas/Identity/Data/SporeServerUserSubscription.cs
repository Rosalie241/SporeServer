using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerUserSubscription
    {
        /// <summary>
        ///     User Subcription Id
        /// </summary>
        [Key]
        public Int64 SubscriptionId { get; set; }

        /// <summary>
        ///     Author Id of user subscription
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Author of user subscription
        /// </summary>
        public SporeServerUser Author { get; set; }

        /// <summary>
        ///     User Id
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        ///     User who Author is subscribed to
        /// </summary>
        public SporeServerUser User { get; set; }
    }
}
