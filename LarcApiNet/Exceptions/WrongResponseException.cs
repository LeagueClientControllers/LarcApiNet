using System;
using System.Collections.Generic;
using System.Text;

namespace LarcApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when method response is incorrect
    /// </summary>
    public class WrongResponseException : Exception
    {
        public WrongResponseException(string methodName) : base($"API method [{methodName}] was executed, but incorrect response was received") { }
    }
}
