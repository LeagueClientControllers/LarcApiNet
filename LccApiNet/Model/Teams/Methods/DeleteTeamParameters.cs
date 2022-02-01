using Newtonsoft.Json;

namespace LccApiNet.Model.Teams.Methods
{
    public class DeleteTeamParameters
    {
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        public DeleteTeamParameters(int teamId)
        {
            TeamId = teamId; 
        }
    }
}
