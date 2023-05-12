/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAssetArcheType
    {
        /// <summary>
        ///     ArcheType Id
        /// </summary>
        [Key]
        public Int64 ArcheTypeId { get; set; }
        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }
        /// <summary>
        ///     Asset
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     ArcheType
        /// </summary>
        public SporeArcheType ArcheType { get; set; }
    }
}
