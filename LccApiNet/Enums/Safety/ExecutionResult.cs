using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Enums.Safety
{
    /// <summary>
    /// All possible results of the API method execution
    /// </summary>
    public sealed class ExecutionResult : SafetyEnum<ExecutionResult>
    {
        /// <summary>
        /// Normal result
        /// </summary>
        public static readonly ExecutionResult Okey = RegisterValue("Okey");

        /// <summary>
        /// Error result
        /// </summary>
        public static readonly ExecutionResult Error = RegisterValue("Error");
    }
}
