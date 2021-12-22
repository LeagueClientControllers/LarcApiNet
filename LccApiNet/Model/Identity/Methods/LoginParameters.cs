using Ardalis.SmartEnum.JsonNet;

using LccApiNet.Model.Device;

using Newtonsoft.Json;

namespace LccApiNet.Model.Identity.Methods
{
    /// <summary>
    /// Parameters of the /identity/login method
    /// </summary>
    public class LoginParameters
    {
        /// <summary>
        /// Type of the current device
        /// </summary>
        [JsonProperty("deviceType")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceType, int>))]
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Name of the current device
        /// </summary>
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        /// <summary>
        /// Nickname or email
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginParameters(string login, string password, DeviceType deviceType, string deviceName)
        {
            Login = login;
            Password = password;
            DeviceType = deviceType;
            DeviceName = deviceName;
        }
    }
}
