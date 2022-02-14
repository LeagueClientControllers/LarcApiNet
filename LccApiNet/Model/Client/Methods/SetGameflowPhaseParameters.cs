using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

namespace LccApiNet.Model.Client.Methods
{
    /// <summary>
    /// Parameters of /client/setGameflowPhase method.
    /// </summary>
    public class SetGameflowPhaseParameters
    {
        /// <summary>
        /// League client game flow phase to set.
        /// </summary>
        [JsonProperty("gameflowPhase")]
        [JsonConverter(typeof(SmartEnumNameConverter<GameflowPhase, int>))]
        public GameflowPhase? GameflowPhase { get; set; }

        /// <summary>
        /// If game flow phase is ready check this property contains time stamp 
        /// when ready check was initiated in UNIX format.
        /// </summary>
        [JsonProperty("readyCheckStarted")]
        public long? ReadyCheckStarted { get; set; }

        public SetGameflowPhaseParameters(GameflowPhase? gameflowPhase, long? readyCheckStarted = null)
        {
            GameflowPhase = gameflowPhase;
            ReadyCheckStarted = readyCheckStarted;
        }
    }
}
