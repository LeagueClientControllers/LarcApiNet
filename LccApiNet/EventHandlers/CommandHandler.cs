using LccApiNet.Model;

using System.Threading.Tasks;

namespace LccApiNet.EventHandlers
{
    public delegate Task<CommandResult> CommandHandler(object sender, CommandSentEventArgs args);
}
