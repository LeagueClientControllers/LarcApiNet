using LarcApiNet.EventHandlers;
using LarcApiNet.Model;

using System.Threading.Tasks;

namespace LarcApiNet.Services
{
    /// <summary>
    /// Represents a function that will execute command asynchronously and return its result to the sender.
    /// </summary>
    public delegate Task<CommandResult> CommandHandler(object sender, Command command);

    /// <summary>
    /// Manage command execution flow.
    /// </summary>
    public class CommandService
    {
        private readonly ILarcApi _api;

        /// <summary>
        /// Handler function of the commands.
        /// </summary>
        public CommandHandler? Handler { private get; set; }

        public CommandService(ILarcApi api)
        {
            _api = api;
            _api.Events.OnCommandEvent += (s, e) => { 
                if (e.Type == CommandEventType.commandSent) {
                    OnCommandSent(s, e.Command!);
                }
            };
        }

        private void OnCommandSent(object sender, Command command)
        {
            if (Handler == null) {
                return;
            }

            CommandResult result = Handler.Invoke(sender, command).Result;
            _api.Client.SetCommandResultAsync(command.Id, result);
        }
    }
}
