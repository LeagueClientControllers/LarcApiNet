using LccApiNet.Model.Client.Commands;
using LccApiNet.Services;

namespace LccApiNet.EventHandlers
{
    /// <summary>
    /// Represents a function that will handle <see cref="UserEventService.CommandSent"/> event.
    /// </summary>
    public delegate void CommandSentEventHandler(object sender, CommandSentEventArgs args);

    /// <summary>
    /// Provides data for <see cref="UserEventService.CommandSent"/> event.
    /// </summary>
    public class CommandSentEventArgs
    {
        /// <summary>
        /// Command that has been sent.
        /// </summary>
        public Command Command { get; set; }

        public CommandSentEventArgs(Command command)
        {
            Command = command;
        }
    }
}
