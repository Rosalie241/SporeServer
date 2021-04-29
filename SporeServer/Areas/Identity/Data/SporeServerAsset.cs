﻿/*
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
using System.ComponentModel.DataAnnotations;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerAsset
    {
        /// <summary>
        ///     The primary key
        /// </summary>
        [Key]
        public Int64 AssetId { get; set; }
        
        /// <summary>
        ///     Parent Asset Id
        /// </summary>
        public Int64 ParentAssetId { get; set; }

        /// <summary>
        ///     Whether it's used
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        ///     Asset Timestamp
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        ///     Asset Author Id
        /// </summary>
        public Int64 AuthorId { get; set; }

        /// <summary>
        ///     Asset Author
        /// </summary>
        public SporeServerUser Author { get; set; }

        /// <summary>
        ///     Name of Asset
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        ///     ModelType of Asset
        /// </summary>
        public SporeModelType ModelType { get; set; }

        /// <summary>
        ///     AssetType of Asset
        /// </summary>
        public SporeAssetType Type { get; set; }

        /// <summary>
        ///     Description of Asset
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Tags of Asset
        /// </summary>
        public string Tags { get; set; }
        
        /// <summary>
        ///     FileSize of PNG
        /// </summary>
        public Int64 Size { get; set; }

        /// <summary>
        ///     Automatically uploaded or not?
        /// </summary>
        public bool Slurped { get; set; }
    }
}
