using LccApiNet.Enums.Safety;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace LccApiNet.Model.General
{
    /// <summary>
    /// Base class for all the response models.
    /// Contains additional fields.
    /// </summary>
    class ApiResponse
    {
        [JsonProperty("result")]
        public ExecutionResult Result; 
    }
}
