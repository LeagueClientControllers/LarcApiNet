using LccApiNet.Model.General;

using Newtonsoft.Json;

using System.Collections.Generic;

namespace LccApiNet.Model.Device.Methods
{
    /// <summary>
    /// Response of the /device/getControllers method.
    /// </summary>
    public class ControllersResponse : ApiResponse
    {
        /// <summary>
        /// List of the controllers.
        /// </summary>
        [JsonProperty("controllers")]
        public ClientController[] Controllers { get; set; } = null!;
    }
}
