using Ardalis.SmartEnum;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Indicates type of the event.
    /// </summary>
    public class ClientEventType : SmartEnum<ClientEventType>
    {
        public ClientEventType(string name, int value) : base(name, value) { }

        /// <summary>
        /// Current league client game flow phase has been changed.
        /// </summary>
        public static readonly ClientEventType GameflowPhaseChanged = new ClientEventType("GameflowPhaseChanged", 1);
    }
}
