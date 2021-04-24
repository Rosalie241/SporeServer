using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
