using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LccApiNet.Model.Client.Methods
{
    public class SetGameflowPhaseParameters
    {
        [JsonProperty("gameflowPhase")]
        [JsonConverter(typeof(StringEnumConverter))]
        public GameflowPhase? GameflowPhase { get; }

        public SetGameflowPhaseParameters(GameflowPhase? gameflowPhase)
        {
            GameflowPhase = gameflowPhase;
        }
    }
}
