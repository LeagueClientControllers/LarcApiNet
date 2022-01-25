using Newtonsoft.Json;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Parameters of the /device/getById parameter
    /// </summary>
    public class GetDeviceParameters
    {
        /// <summary>
        /// Id of the device
        /// </summary>
        [JsonProperty("deviceId")]
        public int DeviceId { get; set; }

        public GetDeviceParameters(int deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
