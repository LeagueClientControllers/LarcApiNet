using Ardalis.SmartEnum;
using Ardalis.SmartEnum.JsonNet;
using LccApiNet.Model.General.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class ChangeMemberRoleParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [JsonProperty("memberId")]
        public int MemberId { get; set; }

        [JsonProperty("role")]
        [JsonConverter(typeof(SmartEnumNameConverter<Role, int>))]
        public Role MemberRole { get; set; }

        public ChangeMemberRoleParameters(int teamId, int memberId, Role role)
        {
            (TeamId, MemberId, MemberRole) = (teamId, memberId, role);
        }
    }
}
