using System;

namespace LarcApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when connecting to event provider or during the process of getting and parsing events.
    /// </summary>
    public class EventProviderException : Exception
    {
        public EventProviderException(string message) : base(message) { }
    }
}