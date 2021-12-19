using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when method response is incorrect
    /// </summary>
    class WrongResponseException : Exception
    {
        public WrongResponseException(string methodName) : base($"API method [{methodName}] was executed, but incorrect response was received") { }
    }
}
