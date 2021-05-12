/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface IAggregatorManager
    {
        /// <summary>
        ///     Adds a new aggregator for author with given name, description and assets, returns null when failed
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<SporeServerAggregator> AddAsync(SporeServerUser author, string name, string description, SporeServerAsset[] assets);

        /// <summary>
        ///     Removes the given aggregator
        /// </summary>
        /// <param name="aggregator"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(SporeServerAggregator aggregator);

        /// <summary>
        ///     Updates the given aggregator
        /// </summary>
        /// <param name="aggregator"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(SporeServerAggregator aggregator);

        /// <summary>
        ///     Tries to find aggregator from id, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeAuthor"></param>
        /// <returns></returns>
        Task<SporeServerAggregator> FindByIdAsync(Int64 id);

        /// <summary>
        ///     Tries to find all aggregators from author, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SporeServerAggregator[]> FindByAuthorAsync(SporeServerUser user);

        /// <summary>
        ///     Returns all aggregators
        /// </summary>
        /// <returns></returns>
        DbSet<SporeServerAggregator> GetAllAggregators();
    }
}
