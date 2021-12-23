using Newtonsoft.Json;

namespace LccApiNet.Model.Teams.Methods
{
    public class ChangeTeamLeaderParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }
        
        [JsonProperty("newLeaderId")]
        public int NewLeaderId { get; set; }

        public ChangeTeamLeaderParameters(int teamId, int newLeaderId)
        {
            (TeamId, NewLeaderId) = (teamId, newLeaderId);
        }
    }
}