using Newtonsoft.Json;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Collection of the events
    /// </summary>
    public class EventCollection
    {
        /// <summary>
        /// All the events related to the user's devices
        /// </summary>
        [JsonProperty("deviceEvents")]
        public DeviceEvent[] DeviceEvents { get; set; } = default!;
    }
}
