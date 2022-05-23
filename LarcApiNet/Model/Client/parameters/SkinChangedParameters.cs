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
    /// 
    /// </summary>
    public class SkinChangedParameters {
        
        /// <summary>
        /// Position of player whose skin has been changed in allies array. [0..4]
        /// </summary>
        [JsonProperty("playerPosition")]
        public int PlayerPosition { get; set; } = default!;
        
        /// <summary>
        /// New skin id.
        /// </summary>
        [JsonProperty("skinId")]
        public int SkinId { get; set; } = default!;
        
        public SkinChangedParameters(int playerPosition, int skinId) {
            this.PlayerPosition = playerPosition;
            this.SkinId = skinId;
        }
    }
}

#nullable restore