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
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace LarcApiNet.Model {
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ProfileInfoResponse : ApiResponse {
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("profile")]
        public ProfileInfo Profile { get; set; } = default!;
    }
}

#nullable restore
