using LccApiNet.Model.General;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Response of the /device/getDevices method
    /// </summary>
    class GetDevicesResponse : ApiResponse
    {
        /// <summary>
        /// List of devices
        /// </summary>
        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }

        public GetDevicesResponse()
        {
            Devices = new List<Device>();
        }
    }
}
