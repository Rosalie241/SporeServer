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
    public interface IUserSubscriptionManager
    {
        /// <summary>
        ///     Adds a new subscription for author, who follows the given user
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> AddAsync(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Removes the given subscription
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(SporeServerUserSubscription subscription);

        /// <summary>
        ///     Finds subscription with user for author, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        SporeServerUserSubscription Find(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Finds all subscriptions for author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        SporeServerUserSubscription[] FindAllByAuthor(SporeServerUser author);
    }
}
