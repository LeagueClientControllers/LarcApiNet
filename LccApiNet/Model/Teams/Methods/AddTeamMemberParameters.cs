using LccApiNet.Model.General.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    internal class AddTeamMemberParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("memberSummonerId")]
        public string MemberSummonerId { get; set; }

        [JsonProperty("memberRole")]
        public Role MemberRole;

        public AddTeamMemberParameters(int teamId, string memberSummonerId, Role memberRole)
        {
            TeamId = teamId;
            MemberSummonerId = memberSummonerId;
            MemberRole = memberRole;
        }

    }
}
