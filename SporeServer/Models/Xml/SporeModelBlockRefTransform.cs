/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public class SporeModelBlockRefTransform
    {
        public double? LimbType { get; set; }
        public double? MuscleScale { get; set; }
        public double? BaseMuscleScale { get; set; }
        public double Scale { get; set; }
        public double[] Position { get; set; }
        public double[] TriangleDirection { get; set; }
        public double[] TrianglePickOrigin { get; set; }
        public SporeModelBlockRefTransformOrientation Orientation { get; set; }
        public SporeModelBlockRefTransformUserOrientation? UserOrientation { get; set; }
    }
}
