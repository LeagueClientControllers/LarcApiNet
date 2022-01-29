using Ardalis.SmartEnum;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Indicates type of the <see cref="CommandEvent"/>
    /// </summary>
    public class CommandEventType : SmartEnum<CommandEventType>
    {
        public CommandEventType(string name, int value) : base(name, value) { }

        /// <summary>
        /// Command has been sent to the client controller.
        /// </summary>
        public static readonly CommandEventType CommandSent = new CommandEventType("CommandSent", 1);

        /// <summary>
        /// Command has been executed in the client controller and result has been sent back to the device.
        /// </summary>
        public static readonly CommandEventType CommandExecuted = new CommandEventType("CommandExecuted", 2);
    }
}
