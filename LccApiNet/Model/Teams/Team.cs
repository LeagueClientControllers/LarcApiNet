using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams
{
    /// <summary>
    /// Represents league team of five members on each of five roles
    /// </summary>
    public class Team
    {
        /// <summary>
        /// Unique id of the team
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the team
        /// <summary>
        [JsonProperty("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Five members of the team
        /// </summary>
        [JsonProperty("members")]
        public Member[] Members { get; set; } = new Member[0];
    }
}
