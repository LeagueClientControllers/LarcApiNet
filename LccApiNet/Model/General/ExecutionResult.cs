using Ardalis.SmartEnum;

namespace LccApiNet.Model.General
{
    /// <summary>
    /// All possible results of the API method execution
    /// </summary>
    public sealed class ExecutionResult : SmartEnum<ExecutionResult>
    {
        public ExecutionResult(string name, int value) : base(name, value) { }

        /// <summary>
        /// Normal result
        /// </summary>
        public static readonly ExecutionResult Okey = new ExecutionResult("Okey", 1);

        /// <summary>
        /// Error result
        /// </summary>
        public static readonly ExecutionResult Error = new ExecutionResult("Error", 2);
    }
}
