using LccApiNet.Model.General;

using Newtonsoft.Json;

namespace LccApiNet.Model.Identity.Methods
{
    /// <summary>
    /// Response of the methods that returns access token
    /// </summary>
    class AccessTokenResponse : ApiResponse
    {
        /// <summary>
        /// Access token given to user
        /// </summary>
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }
    }
}
