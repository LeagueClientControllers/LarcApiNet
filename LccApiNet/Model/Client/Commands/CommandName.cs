using Ardalis.SmartEnum;

namespace LccApiNet.Model.Client.Commands
{
    /// <summary>
    /// Indicates what command should be executed.
    /// </summary>
    public class CommandName : SmartEnum<CommandName>
    {
        public CommandName(string name, int value) : base(name, value) { }
        

        /// <summary>
        /// League match should be accepted.
        /// </summary>
        public static readonly CommandName AcceptMatch = new CommandName("AcceptMatch", 1);

        /// <summary>
        /// League match should be declined.
        /// </summary>
        public static readonly CommandName DeclineMatch = new CommandName("DeclineMatch", 2);
    }
}
