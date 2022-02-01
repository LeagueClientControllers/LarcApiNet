using Newtonsoft.Json;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Collection of the events.
    /// </summary>
    public class EventCollection
    {
        /// <summary>
        /// All of the events related to the user's devices.
        /// </summary>
        [JsonProperty("deviceEvents")]
        public DeviceEvent[] DeviceEvents { get; set; } = null!;

        /// <summary>
        /// All of the events related to the league client.
        /// </summary>
        [JsonProperty("clientEvents")]
        public ClientEvent[] ClientEvents { get; set; } = null!;

        /// <summary>
        /// All of the events related to the command system.
        /// </summary>
        [JsonProperty("commandEvents")]
        public CommandEvent[] CommandEvents { get; set; } = null!;
    }
}
