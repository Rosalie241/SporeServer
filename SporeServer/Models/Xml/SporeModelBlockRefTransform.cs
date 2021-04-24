using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
