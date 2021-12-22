using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.LongPoll.Methods
{
    /// <summary>
    /// Parameters of the /longpoll/getEvents method
    /// </summary>
    internal class LongPollEventsParameters
    {
        /// <summary> 
        /// Determines from which event to start fetching events when executing next long poll request
        /// </summary>
        [JsonProperty("lastEventId")]
        public int LastEventId { get; set; }
    
        /// <summary>
        /// Time within this request will be alive and will listen to events
        /// </summary>
        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        public LongPollEventsParameters(int lastEventId, int timeout = 60)
        {
            this.LastEventId = lastEventId;
            this.Timeout = timeout;
        }
    }
}
