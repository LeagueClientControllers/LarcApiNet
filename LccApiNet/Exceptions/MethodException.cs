using LccApiNet.Model.General;

using System;

namespace LccApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when method was executed with errors.
    /// </summary>
    public class MethodException : Exception
    {
        public string MethodPath { get; }
        public MethodError ErrorName { get; }
        public string ErrorMessage { get; }

        public MethodException(
            string methodPath, 
            MethodError errorName, 
            string errorMessage
        ) : base($"API method [{methodPath}] was executed with error [{errorName}]. Error message - {errorMessage}") {
            MethodPath = methodPath;
            ErrorName = errorName;
            ErrorMessage = errorMessage;
        }
    }
}
