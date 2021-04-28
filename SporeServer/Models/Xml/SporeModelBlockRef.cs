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

namespace SporeServer.Models.Xml
{
    public class SporeModelBlockRef
    {
        public Int64[] BlockId { get; set; }
        public SporeModelBlockRefTransform Transform { get; set; }
        public bool Snapped { get; set; }
        public Int32? PaintListCount { get; set; }
        public SporeModelBlockRefPaint[] PaintList { get; set; }

        public Int32? ChildListCount { get; set; }
        public Int64[] ChildList { get; set; }
        public Int32? Symmetric { get; set; }
        public Int32? HandlesCount { get; set; }
        public SporeModelBlockRefHandle[] Handles { get; set; }
        public bool IsAsymmetric { get; set; }
    }
}
