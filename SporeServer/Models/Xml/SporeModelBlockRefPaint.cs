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
    public class SporeModelBlockRefPaint
    {
        public Int64 PaintRegion { get; set; }
        public Int64 PaintId { get; set; }
        public double[] Color1 { get; set; }
        public double[] Color2 { get; set; }
    }
}
