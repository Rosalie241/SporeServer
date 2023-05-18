using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable annotations

namespace SporeServer.Models.Xml
{
    public class SporeModelScenarioResource
    {
        public Int32? AvatarLocked { get; set; }
        public Int32? AllowedPosseMembers { get; set; }
        public string? WinText { get; set; }
        public string? LoseText { get; set; }
        public string? IntroText { get; set; }
        public Int64? Type { get; set; }
        public Int64[]? AvatarAsset { get; set; }
        public SporeModelScenarioResourceAct[] Acts { get; set; }
        public SporeModelScenarioResourceClass[] Classes { get; set; }
    }
}
