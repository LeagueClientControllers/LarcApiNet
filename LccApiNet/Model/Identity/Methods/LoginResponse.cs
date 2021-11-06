using Newtonsoft.Json;

namespace LccApiNet.Model.Identity.Methods
{
    /// <summary>
    /// Response of the /identity/login method
    /// </summary>
    class LoginResponse
    {
        /// <summary>
        /// Access token given to user
        /// </summary>
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }
    }
}
