using System;

namespace LccApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when internal server error occured when trying to execute API method
    /// </summary>
    public class ApiServerException : Exception
    {
        public ApiServerException() : base("Internal server error occured when trying to execute API method") { }
    }
}
