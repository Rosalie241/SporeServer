using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public class SporeModelScenarioResourceClass
    {
        public SporeModelScenarioResourceClassAct[] Acts { get; set; }
        public string CastName { get; set; }
        public SporeModelScenarioResourceClassAsset Asset { get; set; }
        public Int32? GfxOverrideType { get; set; }
        public Int32? GfxOverideTypeSecondary { get; set; }
        public SporeModelScenarioResourceClassAsset GfxOverrideAsset { get; set; }
    }
}
