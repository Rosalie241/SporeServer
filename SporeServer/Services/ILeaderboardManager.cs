/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using SporeServer.Areas.Identity.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SporeServer.Services
{
    public interface ILeaderboardManager
    {
        /// <summary>
        ///     Adds leaderboard entry for author
        /// </summary>
        /// <returns></returns>
        public Task<bool> AddAsync(SporeServerAsset adventureAsset, SporeServerAsset captainAsset,
                                    Int32 percentageCompleted, Int32 timeInMilliseconds, SporeServerUser author);

        /// <summary>
        ///     Returns all leaderboard entries
        /// </summary>
        /// <returns></returns>
        public DbSet<SporeServerLeaderboardEntry> GetAllEntries();
    }
}
