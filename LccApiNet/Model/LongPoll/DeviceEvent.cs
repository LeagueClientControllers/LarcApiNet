using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Describes event that occured with the device
    /// </summary>
    public class DeviceEvent
    {
        /// <summary>
        /// Type of the event
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceEventType, int>))]
        public DeviceEventType Type { get; set; } = null!;

        /// <summary>
        /// If the device was added, stores id of the new device;
        /// If the device was changed, stores id of the device whose changes invoked this event;
        /// If the device was removed, stores id of the removed device;
        /// </summary>
        [JsonProperty("deviceId")]
        public int DeviceId { get; set; }

        /// <summary>
        /// Stores changes of the device's properties
        /// </summary>
        [JsonProperty("changes")]
        public Dictionary<string, object>? Changes { get; set; }
    }
}
