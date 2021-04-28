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

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public class SporeModelProperties
    {
        public Int64 ModelType { get; set; }
        public Int64? SkinEffect1 { get; set; }
        public Int64? SkinEffect2 { get; set; }
        public Int64? SkinEffect3 { get; set; }
        public Int64? SkinEffectSeed1 { get; set; }
        public Int64? SkinEffectSeed2 { get; set; }
        public Int64? SkinEffectSeed3 { get; set; }
        public double[]? SkinColor1 { get; set; }
        public double[]? SkinColor2 { get; set; }
        public double[]? SkinColor3 { get; set; }
        public Int32? ZcorpScore { get; set; }
        public Int32? Version { get; set; }
    }
}
