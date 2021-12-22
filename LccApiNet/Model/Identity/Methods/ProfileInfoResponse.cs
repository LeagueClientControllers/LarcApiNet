using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Identity.Methods
{
    public class ProfileInfoResponse : ApiResponse
    {
        [JsonProperty("summonerId", NullValueHandling = NullValueHandling.Include)]
        public string? SummomerId { get; set; } = default!;
    }
}
