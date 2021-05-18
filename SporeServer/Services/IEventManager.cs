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
    public interface IEventManager
    {
        /// <summary>
        ///     Adds Event for author
        /// </summary>
        /// <param name="eventStream"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        Task<bool> AddAsync(Stream eventStream, SporeServerUser author);
    }
}
