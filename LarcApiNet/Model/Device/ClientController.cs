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
    /// Representation of the device that is owned by the user
    /// if it's computer that controls league of legends client
    /// </summary>
    public class ClientController : BindableBase {
        
        /// <summary>
        /// Identifier of the controller.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; } = default!;
        
        private string _name = default!;
        
        private bool _isOnline = default!;
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("client")]
        public LeagueClient? Client { get; set; }//;
        
        /// <summary>
        /// Name of the controller.
        /// </summary>
        [JsonProperty("name")]
        public string Name {
            get {
                return _name;
            }
            set {
                this.SetProperty(ref _name, value);
            }
        }
        
        /// <summary>
        /// Whether the device is online
        /// </summary>
        [JsonProperty("isOnline")]
        public bool IsOnline {
            get {
                return _isOnline;
            }
            set {
                this.SetProperty(ref _isOnline, value);
            }
        }
    }
}

#nullable restore
