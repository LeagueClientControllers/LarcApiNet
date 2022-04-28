using System;

namespace LarcApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when internal server error occurred when trying to execute API method
    /// </summary>
    public class ApiServerException : Exception
    {
        public ApiServerException() : base("Internal server error occurred when trying to execute API method") { }
    }
}
