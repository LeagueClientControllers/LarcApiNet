using System;

namespace LarcApiNet.Exceptions
{
    /// <summary>
    /// The exception that occurred when API server host reject incoming request
    /// </summary>
    public class ServerUnreachableException : Exception
    {
        public ServerUnreachableException() : base("Unable to connect to API server") { }
    }
}
