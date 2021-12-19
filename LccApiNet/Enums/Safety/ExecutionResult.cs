namespace LccApiNet.Enums.Safety
{
    /// <summary>
    /// All possible results of the API method execution
    /// </summary>
    public sealed class ExecutionResult : SafetyEnum<ExecutionResult>
    {
        public ExecutionResult()
        {
            Init();
        }

        /// <summary>
        /// Normal result
        /// </summary>
        [SafetyEnumValue("Okey")]
        public static ExecutionResult Okey { get; private set; } = null!;

        /// <summary>
        /// Error result
        /// </summary>
        [SafetyEnumValue("Error")]
        public static ExecutionResult Error { get; private set; } = null!;
    }
}
