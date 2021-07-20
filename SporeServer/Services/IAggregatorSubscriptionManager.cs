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
    public interface IAggregatorSubscriptionManager
    {
        /// <summary>
        ///     Adds a new subscription for author, who follows the given aggregator
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> AddAsync(SporeServerUser author, SporeServerAggregator aggregator);

        /// <summary>
        ///     Removes the given subscription
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(SporeServerAggregatorSubscription subscription);

        /// <summary>
        ///     Finds subscription with user for author, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<SporeServerAggregatorSubscription> FindAsync(SporeServerUser author, SporeServerAggregator aggregator);

        /// <summary>
        ///     Finds all subscriptions for author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        Task<SporeServerAggregatorSubscription[]> FindAllByAuthorAsync(SporeServerUser author);

        /// <summary>
        ///     Gets subscruber count for aggregator
        /// </summary>
        /// <param name="aggregator"></param>
        /// <returns></returns>
        Task<Int32> GetSubscriberCountAsync(SporeServerAggregator aggregator);
    }
}
