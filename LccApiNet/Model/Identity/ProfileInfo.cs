using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LccApiNet.Model.Identity
{
    public class ProfileInfo
    {
        [JsonProperty("summonerId", NullValueHandling = NullValueHandling.Include)]
        public string? SummomerId { get; set; } = default!;

    }
}
