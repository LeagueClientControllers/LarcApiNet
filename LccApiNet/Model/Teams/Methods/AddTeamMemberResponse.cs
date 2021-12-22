using LccApiNet.Model.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.Teams.Methods
{
    public class AddTeamMemberResponse : ApiResponse
    {
        [JsonProperty("memberId")]
        public int MemberId { get; set; }

        public AddTeamMemberResponse(int memberId)
        {
            MemberId = memberId;
        }
    }
}
