using LccApiNet.Model.General;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.LongPoll.Methods
{
    /// <summary>
    /// Response of the /longpoll/getLastEventId method
    /// </summary>
    public class EventIdResponse : ApiResponse
    {
        /// <summary>
        /// Id of the last event
        /// </summary>
        [JsonProperty("lastEventId")]
        public int LastEventId { get; set; }
    }
}
