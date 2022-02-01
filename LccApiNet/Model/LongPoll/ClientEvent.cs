using Newtonsoft.Json;

using System.Collections.Generic;
using System.Data;
using System.Security;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Describes an event that is related to the league client.
    /// </summary>
    public class ClientEvent
    {
        /// <summary>
        /// Type of this event.
        /// </summary>
        [JsonProperty("type")]
        public ClientEventType Type { get; set; } = null!;

        /// <summary>
        /// Id of the controller that is controlling the client we receive changes of.
        /// </summary>
        [JsonProperty("controllerId")]
        public int ControllerId { get; set; }

        /// <summary>
        /// Changes of the client properties.
        /// </summary>
        [JsonProperty("changes")]
        public Dictionary<string, object>? Changes { get; set; }
    }
}
