using LccApiNet.Model.General;

using Newtonsoft.Json;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Response of the /device/getControllerById method.
    /// </summary>
    public class ControllerResponse : ApiResponse
    {
        /// <summary>
        /// Controller object with requested id.
        /// </summary>
        [JsonProperty("controller")]
        public ClientController Controller { get; set; } = null!;
    }
}
