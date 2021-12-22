using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class DeleteTeamParameters
    {
        [JsonProperty("teamId")]
        public int DeleteTeamId { get; set; }

        public DeleteTeamParameters(int teamId)
        {
            DeleteTeamId = teamId; 
        }
    }
}
