using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    /// <summary>
    /// 
    /// </summary>
    public class TeamsResponse : ApiResponse
    {
        [JsonProperty("teams")]
        public Team[] Teams { get; set; } = default!;
    }
}
