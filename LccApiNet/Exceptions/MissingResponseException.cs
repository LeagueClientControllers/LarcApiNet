using System;

namespace LccApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when API method response is required, but was not received
    /// </summary>
    public class MissingResponseException : Exception
    {
        public MissingResponseException(string methodPath) : base($"API method [{methodPath}] was executed with no response") { }
    }
}
