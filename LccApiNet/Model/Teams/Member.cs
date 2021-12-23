using Ardalis.SmartEnum.JsonNet;
using LccApiNet.Model.General.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams
{
    /// <summary>
    /// Represents one of the five members of the league team
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Unique id of the team member
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Id of the Riot account associated with the user
        /// </summary>
        [JsonProperty("summonerId")]
        public string SummonerId { get; set; } = default!;

        /// <summary>
        /// Role of the team member
        /// </summary>
        [JsonProperty("teamRole")]
        [JsonConverter(typeof(SmartEnumNameConverter<Role, int>))]
        public Role TeamRole { get; set; } = default!;

        [JsonProperty("isLeader")]
        public bool IsLeader { get; set; }
    }
}
