using LccApiNet.Model.General;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Response of the /device/getDevices method
    /// </summary>
    class DevicesResponse : ApiResponse
    {
        /// <summary>
        /// List of devices
        /// </summary>
        [JsonProperty("devices")]
        public List<DeviceModel> Devices { get; set; }

        public DevicesResponse()
        {
            Devices = new List<DeviceModel>();
        }
    }
}
