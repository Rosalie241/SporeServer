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
    public interface IRatingManager
    {
        /// <summary>
        ///     Adds a rating with rating for asset from author
        /// </summary>
        /// <param name="author"></param>
        /// <param name="asset"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        Task<bool> AddAsync(SporeServerUser author, SporeServerAsset asset, bool rating);

        /// <summary>
        ///     Finds the rating from user for asset, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <param name="asset"></param>
        /// <returns></returns>
        Task<SporeServerRating> FindAsync(SporeServerUser author, SporeServerAsset asset);

        /// <summary>
        ///     Finds all ratings for asset, returns null when none are found
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        Task<SporeServerRating[]> FindAllByAssetAsync(SporeServerAsset asset);

        /// <summary>
        ///     Updates the given rating
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(SporeServerRating rating);
    }
}
