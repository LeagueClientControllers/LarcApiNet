using Ardalis.SmartEnum.JsonNet;

using LccApiNet.Model.Client.Commands;

using Newtonsoft.Json;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Describes an event that is related to the command system.
    /// </summary>
    public class CommandEvent
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(SmartEnumNameConverter<CommandEventType, int>))]
        public CommandEventType Type { get; set; } = null!;

        /// <summary>
        /// If <see cref="Type"/> is <see cref="CommandEventType.CommandSent"/>,
        /// contains information about sent command.
        /// </summary>
        [JsonProperty("command")]
        public Command? Command { get; set; }

        /// <summary>
        /// If <see cref="Type"/> is <see cref="CommandEventType.CommandExecuted"/>,
        /// contains id of the command.
        /// </summary>
        [JsonProperty("commandId")]
        public int CommandId { get; set; } = default!;

        /// <summary>
        /// If <see cref="Type"/> is <see cref="CommandEventType.CommandExecuted"/>,
        /// contains result of the command.
        /// </summary>
        [JsonProperty("commandResult")]
        public CommandResult? CommandResult { get; set; }
    }
}
