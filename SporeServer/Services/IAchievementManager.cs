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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface IAchievementManager
    {
        /// <summary>
        ///     Unlocks achievement with achievement id for author
        /// </summary>
        /// <param name="achievementId"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public Task<bool> UnlockAsync(Int64 achievementId, SporeServerUser author);

        /// <summary>
        ///     Finds all unlocked achievements for author (returns achievement ids)
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Task<Int64[]> FindAllByAuthorAsync(SporeServerUser author);
    }
}
