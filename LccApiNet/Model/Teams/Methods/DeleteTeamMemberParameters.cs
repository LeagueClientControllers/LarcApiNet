using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class DeleteTeamMemberParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }
     
        [JsonProperty("membeId")]
        public int MemberId { get; set; }

        public DeleteTeamMemberParameters(int teamId, int memberId)
        {
            (TeamId, MemberId) = (teamId, memberId);
        }
    }
}
