/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerLeaderboardEntry
    {
        /// <summary>
        ///		Leaderboard Entry Id
        /// </summary>
        [Key]
        public Int64 EntryId { get; set; }
        /// <summary>
        ///     Leaderboard Entry Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Leaderboard Entry Asset (adventure)
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Leaderboard Entry Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Leaderboard Entry Author
        /// </summary>
        public SporeServerUser Author { get; set; }
        /// <summary>
        ///     Leaderboard Entry Timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        ///     Leaderboard Entry Percentage Completed (0 - 1000)
        /// </summary>
        public Int32 PercentageCompleted { get; set; }
        /// <summary>
        ///     Leaderboard Entry Time In Milliseconds
        /// </summary>
        public Int64 TimeInMilliseconds { get; set; }
        /// <summary>
        ///     Leaderboard Entry Captain Id
        /// </summary>
        public Int64 CaptainId { get; set; }
        /// <summary>
        ///     Leaderboard Entry Captain
        /// </summary>
        public SporeServerAsset Captain { get; set; }
    }
}
