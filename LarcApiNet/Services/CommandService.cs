// ---------------------------------------------------------------------------------------------------------------------------------------------------------------
// THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS CREATIVE COMMONS «ATTRIBUTION-NONCOMMERCIAL-NODERIVATIVES» 4.0 WORLDWIDE LICENSE.
// THE WORK IS PROTECTED BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.
// BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND AGREE TO BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE MAY BE CONSIDERED 
// TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
// TO VIEW A COPY OF THIS LICENSE, VISIT HTTP://CREATIVECOMMONS.ORG/LICENSES/BY-NC-ND/4.0/.
// ---------------------------------------------------------------------------------------------------------------------------------------------------------------

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
