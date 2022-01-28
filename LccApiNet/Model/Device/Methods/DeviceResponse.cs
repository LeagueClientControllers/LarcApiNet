using LccApiNet.Model.General;
using Newtonsoft.Json;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Response containing one device
    /// </summary>
    public class DeviceResponse : ApiResponse
    {
        /// <summary>
        /// Devices contained by the response
        /// </summary>
        [JsonProperty("device")]
        public Device Device { get; set; } = default!;
    }
}
