using LccApiNet.EventHandlers;
using LccApiNet.Model.Client.Commands;

namespace LccApiNet.Core.Services
{
    /// <summary>
    /// Manage command execution flow.
    /// </summary>
    public class CommandService
    {
        private ILccApi _api;

        /// <summary>
        /// Handler function of the commands.
        /// </summary>
        public CommandHandler? Handler { private get; set; }
        
        public CommandService(ILccApi api)
        {
            _api = api;
            _api.UserEvents.CommandSent += OnCommandSent;
        }

        private void OnCommandSent(object sender, CommandSentEventArgs args)
        {
            if (Handler == null) {
                return;
            }

            CommandResult result = Handler.Invoke(sender, args).Result;
            _api.Client.SetCommandResult(args.Command.Id, result);
        }
    }
}
