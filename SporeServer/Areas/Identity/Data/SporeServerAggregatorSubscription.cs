using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAggregatorSubscription
    {
        /// <summary>
        ///     Aggregator Subcription Id
        /// </summary>
        [Key]
        public Int64 SubscriptionId { get; set; }
        /// <summary>
        ///     Author Id of aggregator subscription
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Author of aggregator subscription
        /// </summary>
        public SporeServerUser Author { get; set; }
        /// <summary>
        ///     Aggregator Id
        /// </summary>
        public Int64 AggregatorId { get; set; }
        /// <summary>
        ///     Aggregator which Author is subscribed to
        /// </summary>
        public SporeServerAggregator Aggregator { get; set; }
    }
}
