using Ardalis.SmartEnum.JsonNet;

using LccApiNet.Model.Client;

using Newtonsoft.Json;

namespace LccApiNet.Model.Device
{
    /// <summary>
    /// Describes controller - desktop application that is controlling league client.
    /// </summary>
    public class ClientController
    {
        /// <summary>
        /// Identifier of the controller.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the controller.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Whether this controller is long polling events.
        /// </summary>
        [JsonProperty("isOnline")]
        public bool IsOnline { get; set; }

        /// <summary>
        /// Current game flow phase of the league client.
        /// </summary>
        [JsonProperty("gameflowPhase")]
        [JsonConverter(typeof(SmartEnumNameConverter<GameflowPhase, int>))]
        GameflowPhase? GameflowPhase { get; set; }
    }
}
