﻿using LccApiNet.Enums.Safety;
using LccApiNet.Utilities.JsonConverters;

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
        public int? UserId { get; set; }

        /// <summary>
        /// Name of the user that can be authorized with this token
        /// </summary>
        [JsonProperty("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Id of the current device
        /// </summary>
        [JsonProperty("deviceId")]
        public int? DeviceId { get; set; }

        /// <summary>
        /// Type of the current device
        /// </summary>
        [JsonProperty("deviceType")]
        [JsonConverter(typeof(SafetyEnumConverter<DeviceType>))]
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// When token was issued
        /// </summary>
        [JsonProperty("iat")]
        public int? IssuedAt { get; set; }

        /// <summary>
        /// When token is expired
        /// </summary>
        [JsonProperty("exp")]
        public int? ExpireAt { get; set; }
    }
}
