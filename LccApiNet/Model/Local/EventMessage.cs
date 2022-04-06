using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LccApiNet.Model.Local
{
    public class EventMessage
    {
        [JsonProperty("eventType")]
        [JsonConverter(typeof(SmartEnumNameConverter<EventType, int>))]
        public EventType Type { get; set; } = null!;

        [JsonProperty("event")]
        public JObject Event { get; set; } = null!;
    }
}