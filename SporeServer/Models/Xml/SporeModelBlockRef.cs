using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
