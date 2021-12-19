using LccApiNet.Enums.Safety;
using LccApiNet.Utilities.JsonConverters;

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
        [JsonConverter(typeof(SafetyEnumConverter<ExecutionResult>))]
        public ExecutionResult? Result { get; set; } 
    
        /// <summary>
        /// Name of the error occured during execution
        /// </summary>
        [JsonProperty("errorName")]
        [JsonConverter(typeof(SafetyEnumConverter<MethodError>))]
        public MethodError? ErrorName { get; set; }

        /// <summary>
        /// Message of the error occured during execution
        /// </summary>
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}
