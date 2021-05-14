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

namespace SporeServer.SporeTypes
{
    public struct SporeAchievement
    {
        /// <summary>
        ///     Id of achievement
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        ///     Name of Achievement
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     Description of Achievement
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     FileName when Achievement is unlocked
        /// </summary>
        public string UnlockedFileNameHash { get; set; }
        /// <summary>
        ///     FileName when Achievement is locked
        /// </summary>
        public string LockedFileNameHash { get; set; }
        /// <summary>
        ///     Whether the Achievement is secret or not
        /// </summary>
        public bool Secret { get; set; }
    }
}
