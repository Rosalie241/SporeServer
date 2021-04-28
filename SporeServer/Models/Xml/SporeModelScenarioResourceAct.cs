using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SporeServer.Models.Xml
{
    public class SporeModelScenarioResourceAct
    {
        public SporeModelScenarioResourceActGoal[] Goals { get; set; }
        public Int32? TimeLimitSecs { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32? TimeVisible { get; set; }
        public Int64? MusicId { get; set; }
    }
}
