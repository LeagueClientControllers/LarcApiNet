#nullable enable
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ardalis.SmartEnum.JsonNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace LarcApiNet.Model {
    
    
    /// <summary>
    /// Payload of the access token
    /// </summary>
    public class JwtPayload {
        
        /// <summary>
        /// Id of the authorized user
        /// </summary>
        [JsonProperty("userId")]
        public int UserId { get; set; } = default!;
        
        /// <summary>
        /// Name of the authorized user
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; } = default!;
        
        /// <summary>
        /// Id of the device that is authorized under the user
        /// </summary>
        [JsonProperty("deviceId")]
        public int DeviceId { get; set; } = default!;
        
        /// <summary>
        /// Type of the device that is authorized under the user
        /// </summary>
        [JsonProperty("deviceType")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceType, int>))]
        public DeviceType DeviceType { get; set; } = default!;
        
        /// <summary>
        /// Time when the token was issued
        /// </summary>
        [JsonProperty("iat")]
        public int Iat { get; set; } = default!;
        
        /// <summary>
        /// Time when the token will be expired
        /// </summary>
        [JsonProperty("exp")]
        public int Exp { get; set; } = default!;
    }
}

#nullable restore
