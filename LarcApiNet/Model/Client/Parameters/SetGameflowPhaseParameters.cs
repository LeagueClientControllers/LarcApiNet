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
    /// Parameters of /client/setGameflowPhase method.
    /// </summary>
    public class SetGameflowPhaseParameters {
        
        /// <summary>
        /// Current league client game flow phase to set.
        /// </summary>
        [JsonProperty("gameflowPhase")]
        [JsonConverter(typeof(SmartEnumNameConverter<GameflowPhase, int>))]
        public GameflowPhase? GameflowPhase { get; set; }//;
        
        /// <summary>
        /// If game flow phase is ready check, this property determines timestamp when ready check was started in unix format.
        /// </summary>
        [JsonProperty("readyCheckStarted")]
        public int? ReadyCheckStarted { get; set; }//;
        
        public SetGameflowPhaseParameters(GameflowPhase? gameflowPhase, int? readyCheckStarted) {
            this.GameflowPhase = gameflowPhase;
            this.ReadyCheckStarted = readyCheckStarted;
        }
    }
}

#nullable restore
