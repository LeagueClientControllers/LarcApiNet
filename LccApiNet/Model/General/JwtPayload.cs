using Ardalis.SmartEnum.JsonNet;

using LccApiNet.Model.Device;

using Newtonsoft.Json;

namespace LccApiNet.Model.General
{
    /// <summary>
    /// Represents the content of jwt payload
    /// </summary>
    public class JwtPayload
    {
        /// <summary>
        /// Id of the user that can be authorized with this token
        /// </summary>
        [JsonProperty("userId")]
        public int UserId { get; set; } = default!;

        /// <summary>
        /// Name of the user that can be authorized with this token
        /// </summary>
        [JsonProperty("username")]
        public string? Username { get; set; } = null!;

        /// <summary>
        /// Id of the current device
        /// </summary>
        [JsonProperty("deviceId")]
        public int DeviceId { get; set; } = default!;

        /// <summary>
        /// Type of the current device
        /// </summary>
        [JsonProperty("deviceType")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceType, int>))]
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// When token was issued
        /// </summary>
        [JsonProperty("iat")]
        public int IssuedAt { get; set; } = default!;

        /// <summary>
        /// When token is expired
        /// </summary>
        [JsonProperty("exp")]
        public int ExpireAt { get; set; } = default!;
    }
}
