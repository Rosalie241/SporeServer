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
using Microsoft.AspNetCore.Http;

namespace SporeServer.Models
{
    public class SnapshotUploadForm
    {
        /// <summary>
        ///     Email Locale
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        ///     From Email Address
        /// </summary>
        public string MyEmail { get; set; }
        /// <summary>
        ///     Destination Email Address
        /// </summary>
        public string DestEmail { get; set; }
        /// <summary>
        ///     Email Message Text
        /// </summary>
        public string MsgText { get; set; }
        /// <summary>
        ///     Email Image Data
        /// </summary>
        public IFormFile Imagedata { get; set; }
    }
}