/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Http;
using System;

namespace SporeServer.Models
{
    public class AssetUploadForm
    {
        /// <summary>
        ///     Asset Type Id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        ///     Asset Id
        /// </summary>
        public Int64 AssetId { get; set; }

        //public int[] traitguids { get; set; }
        
        /// <summary>
        ///     Asset Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Asset Tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        ///     Asset Xml,
        ///     filename is asset name
        /// </summary>
        public IFormFile ModelData { get; set; }
        
        /// <summary>
        ///     Asset PNG
        /// </summary>
        public IFormFile ThumbnailData { get; set; }

        /// <summary>
        ///     Image 1
        /// </summary>
        public IFormFile ImageData { get; set; }

        /// <summary>
        ///     Image 2
        /// </summary>
        public IFormFile ImageData_2 { get; set; }

        /// <summary>
        ///     Image 3
        /// </summary>
        public IFormFile ImageData_3 { get; set; }

        /// <summary>
        ///     Image 4
        /// </summary>
        public IFormFile ImageData_4 { get; set; }
    }
}
