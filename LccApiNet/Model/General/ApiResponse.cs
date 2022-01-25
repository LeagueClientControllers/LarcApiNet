using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;

namespace LccApiNet.Model.General
{
    /// <summary>
    /// Base class for all the response models
    /// Contains basic response fields
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Result of the response
        /// </summary>
        [JsonProperty("result")]
        [JsonConverter(typeof(SmartEnumNameConverter<ExecutionResult, int>))]
        public ExecutionResult? Result { get; set; } 
    
        /// <summary>
        /// Name of the error occurred during execution
        /// </summary>
        [JsonProperty("errorName")]
        [JsonConverter(typeof(SmartEnumNameConverter<MethodError, int>))]
        public MethodError? ErrorName { get; set; }

        /// <summary>
        /// Message of the error occurred during execution
        /// </summary>
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}
