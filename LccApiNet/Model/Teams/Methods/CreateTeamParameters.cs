using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class CreateTeamParameters : ApiResponse
    {
        [JsonProperty("teamName")]
        public string Name { get; set; }

        public CreateTeamParameters(string name)
        {
            Name = name;
        }
    }
}
