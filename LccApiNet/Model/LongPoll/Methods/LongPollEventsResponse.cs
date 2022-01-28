using LccApiNet.Model.General;

using Newtonsoft.Json;

namespace LccApiNet.Model.LongPoll.Methods
{
    /// <summary>
    /// Response of the /longpoll/getEvents method
    /// </summary>
    public class LongPollEventsResponse : ApiResponse
    {
        /// <summary>
        /// Id of the last event in the returned collection. 
        /// Used to point out when to start catch events when executing next long poll request
        /// </summary>
        [JsonProperty("lastEventId")]
        public int LastEventId { get; set; }

        /// <summary>
        /// Collection of the events
        /// </summary>
        [JsonProperty("events")]
        public EventCollection Events { get; set; } = default!;
    }
}
