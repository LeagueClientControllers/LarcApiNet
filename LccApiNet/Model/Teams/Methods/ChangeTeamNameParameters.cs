using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LccApiNet.Model.Teams.Methods
{
    public class ChangeTeamNameParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("teamName")]
        public string TeamName { get; set; }

        public ChangeTeamNameParameters(int teamId, string teamName)
        {
            (TeamName, TeamId) = (teamName, teamId);
        }
    }
}
