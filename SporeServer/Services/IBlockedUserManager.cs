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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface IBlockedUserManager
    {
        /// <summary>
        ///     Adds block for user from author
        /// </summary>
        /// <param name="author"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddAsync(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Removes block
        /// </summary>
        /// <param name="blockedUser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(SporeServerBlockedUser blockedUser);

        /// <summary>
        ///     Finds the block for user from author, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<SporeServerBlockedUser> FindAsync(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Finds all blocked users from author, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        Task<SporeServerBlockedUser[]> FindAllByAuthorAsync(SporeServerUser author);

    }
}
