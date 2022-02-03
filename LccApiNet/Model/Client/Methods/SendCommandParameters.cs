using LccApiNet.Model.Client.Commands;

using Newtonsoft.Json;

using System.Text.Json.Serialization;

namespace LccApiNet.Model.Client.Methods
{
    /// <summary>
    /// Parameters for the /client/sendCommand method.
    /// </summary>
    public class SendCommandParameters
    {
        /// <summary>
        /// Id of the controller provided command is destined for.
        /// </summary>
        [JsonProperty("controllerId")]
        public int ControllerId { get; set; }

        /// <summary>
        /// Name of the command for the client controller.
        /// </summary>
        [JsonProperty("commandName")]
        public CommandName CommandName { get; set; }

        public SendCommandParameters(int controllerId, CommandName commandName)
        {
            ControllerId = controllerId;
            CommandName = commandName;
        }
    }
}
