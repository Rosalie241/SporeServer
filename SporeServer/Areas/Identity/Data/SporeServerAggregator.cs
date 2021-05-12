using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAggregator
    {
        /// <summary>
        ///     Aggregator Id
        /// </summary>
        [Key]
        public Int64 AggregatorId { get; set; }
        /// <summary>
        ///     Aggregator Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Aggregator Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     Aggregator Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        ///     Aggregator Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Aggregator Author
        /// </summary>
        public SporeServerUser Author { get; set; }
        /// <summary>
        ///     Aggregator SubscriberCount
        /// </summary>
        public int SubscriberCount { get; set; }
        /// <summary>
        ///     Aggregator Assets
        /// </summary>
        public ICollection<SporeServerAsset> Assets { get; set; }
    }
}
