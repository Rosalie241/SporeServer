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
using System.ComponentModel.DataAnnotations;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAssetComment
    {
        /// <summary>
        ///     Comment Id
        /// </summary>
        [Key]
        public Int64 CommentId { get; set; }
        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Asset
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Author
        /// </summary>
        public SporeServerUser Author { get; set; }
        /// <summary>
        ///     Whether it's approved by the asset author
        /// </summary>
        public bool Approved { get; set; }
        /// <summary>
        ///     Comment Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        ///     Comment
        /// </summary>
        public string Comment { get; set; }
    }
}
