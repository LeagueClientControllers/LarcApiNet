using Ardalis.SmartEnum.JsonNet;

using LccApiNet.Model.General;

using Newtonsoft.Json;

using System;

namespace LccApiNet.Model.Client.Commands
{
    /// <summary>
    /// Describes result of the command.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Indicates result of the command execution.
        /// </summary>
        [JsonProperty("result")]
        [JsonConverter(typeof(SmartEnumNameConverter<ExecutionResult, int>))]
        public ExecutionResult Result { get; set; }

        /// <summary>
        /// Indicates what error occurred during command execution.
        /// </summary>
        [JsonProperty("error")]
        [JsonConverter(typeof(SmartEnumNameConverter<CommandError, int>))]
        public CommandError? Error { get; set; }

        /// <summary>
        /// Message of the error occurred during command execution.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Creates new command result.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public CommandResult(ExecutionResult result, CommandError? error = null, string? errorMessage = null)
        {
            if (result == ExecutionResult.Error && (error == null || errorMessage == null)) {
                throw new ArgumentException("Error and error message arguments shouldn't be null if execution result is 'Error'");
            }

            Result = result;
            Error = error;
            ErrorMessage = errorMessage;
        }
    }
}
