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
using Microsoft.AspNetCore.Identity;

namespace SporeServer.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SporeServerUser class
    public class SporeServerUser : IdentityUser<Int64>
    {
        /// <summary>
        ///     Reserved Next Asset Id
        /// </summary>
        public Int64 NextAssetId { get; set; }

    }
}
