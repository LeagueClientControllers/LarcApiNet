using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

namespace LccApiNet.Model.Device
{
    /// <summary>
    /// Represents remote device that is used to control league client
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Unique identificator of the device
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Type of the device
        /// </summary>
        [JsonProperty("deviceType")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceType, int>))]
        public DeviceType Type { get; set; } = null!;

        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Local IP address of the devices
        /// </summary>
        public bool IsOnline { get; set; }
    }
}
