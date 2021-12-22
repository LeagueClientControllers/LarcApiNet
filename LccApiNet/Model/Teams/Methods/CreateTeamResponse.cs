using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class CreateTeamResponse : ApiResponse
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }
    }
}
