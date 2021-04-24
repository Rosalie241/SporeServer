using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Int32 ZcorpScore { get; set; }
    }
}
