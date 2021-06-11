/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerRating
    {
        /// <summary>
        ///     Rating Id
        /// </summary>
        [Key]
        public Int64 RatingId { get; set; }
        /// <summary>
        ///     Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Author
        /// </summary>
        public SporeServerUser Author { get; set; }
        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Asset
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Rating
        /// </summary>
        public bool Rating { get; set; }
    }
}
