using Ardalis.SmartEnum;

namespace LccApiNet.Model.Client.Commands
{
    /// <summary>
    /// Indicates type of the error occurred during execution of the command.
    /// </summary>
    public class CommandError : SmartEnum<CommandError>
    {
        public CommandError(string name, int value) : base(name, value) { }

        /// <summary>
        /// Error occurred due to the attempt to accept or decline match when game flow phase is not 'ReadyCheck'.
        /// </summary>
        public static readonly CommandError ReadyCheckError = new CommandError("ReadyCheckError", 1);
    }
}
