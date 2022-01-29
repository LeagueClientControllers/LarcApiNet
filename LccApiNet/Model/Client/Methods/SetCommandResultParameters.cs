using LccApiNet.Model.Client.Commands;

using Newtonsoft.Json;

namespace LccApiNet.Model.Client.Methods
{
    /// <summary>
    /// Parameters of the /client/setCommandResult method.
    /// </summary>
    public class SetCommandResultParameters
    {
        /// <summary>
        /// Id of the command.
        /// </summary>
        [JsonProperty("commandId")]
        public int CommandId { get; set; }

        /// <summary>
        /// Result of the command.
        /// </summary>
        [JsonProperty("result")]
        public CommandResult Result { get; set; }

        public SetCommandResultParameters(int commandId, CommandResult result)
        {
            CommandId = commandId;
            Result = result;
        }
    }
}
