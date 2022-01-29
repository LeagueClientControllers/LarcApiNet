using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

namespace LccApiNet.Model.Client.Commands
{
    /// <summary>
    /// Describes command that is sent to the client controller to execute.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Identifier of the command.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the command.
        /// </summary>
        [JsonProperty("name")]
        [JsonConverter(typeof(SmartEnumNameConverter<CommandName, int>))]
        public CommandName Name { get; set; } = null!;

        /// <summary>
        /// Arguments for the command.
        /// </summary>
        [JsonProperty("args")]
        public object? Args { get; set; }
    }
}
