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
    /// Represents select phase in league client
    /// with the data about teams, summoners, champions, bans etc.
    /// </summary>
    public class PickStage : BindableBase {
        
        /// <summary>
        /// Index of the allies array that corresponds to the user.
        /// </summary>
        [JsonProperty("userPosition")]
        public int UserPosition { get; set; } = default!;
        
        private List<DateTime>? _banRequested;
        
        private List<DateTime>? _pickRequested;
        
        private DateTime? _prepareStageStarted;
        
        private List<ActionType>? _actionType;
        
        private List<bool>? _isActorAnAlly;
        
        private List<int>? _firstActorPosition;
        
        private List<int>? _actorsCount;
        
        /// <summary>
        /// Members of the user's team.
        /// Index in the array points to player position in the pick order.
        /// </summary>
        [JsonProperty("allies")]
        public List<Summoner> Allies { get; set; } = default!;
        
        /// <summary>
        /// Members of an opposing team.
        /// Index in the array points to player position in the pick order.
        /// </summary>
        [JsonProperty("enemies")]
        public List<Summoner> Enemies { get; set; } = default!;
        
        /// <summary>
        /// Time when ban phase was started.
        /// </summary>
        [JsonProperty("banRequested")]
        public List<DateTime>? BanRequested {
            get {
                return _banRequested;
            }
            set {
                this.SetProperty(ref _banRequested, value);
            }
        }
        
        /// <summary>
        /// Time when pick phase was started.
        /// </summary>
        [JsonProperty("pickRequested")]
        public List<DateTime>? PickRequested {
            get {
                return _pickRequested;
            }
            set {
                this.SetProperty(ref _pickRequested, value);
            }
        }
        
        /// <summary>
        /// Time when prepare phase was started.
        /// </summary>
        [JsonProperty("prepareStageStarted")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? PrepareStageStarted {
            get {
                return _prepareStageStarted;
            }
            set {
                this.SetProperty(ref _prepareStageStarted, value);
            }
        }
        
        /// <summary>
        /// Type of the current action.
        /// </summary>
        [JsonProperty("actionType")]
        public List<ActionType>? ActionType {
            get {
                return _actionType;
            }
            set {
                this.SetProperty(ref _actionType, value);
            }
        }
        
        /// <summary>
        /// Is current action prescribed for the user's ally.
        /// </summary>
        [JsonProperty("isActorAnAlly")]
        public List<bool>? IsActorAnAlly {
            get {
                return _isActorAnAlly;
            }
            set {
                this.SetProperty(ref _isActorAnAlly, value);
            }
        }
        
        /// <summary>
        /// Index of the allies or opponents array that points
        /// to the first summoner that participate in the current action.
        /// </summary>
        [JsonProperty("firstActorPosition")]
        public List<int>? FirstActorPosition {
            get {
                return _firstActorPosition;
            }
            set {
                this.SetProperty(ref _firstActorPosition, value);
            }
        }
        
        /// <summary>
        /// Number of participants of the current action.
        /// </summary>
        [JsonProperty("actorsCount")]
        public List<int>? ActorsCount {
            get {
                return _actorsCount;
            }
            set {
                this.SetProperty(ref _actorsCount, value);
            }
        }
    }
}

#nullable restore