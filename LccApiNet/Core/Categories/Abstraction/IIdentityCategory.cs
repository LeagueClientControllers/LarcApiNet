using LccApiNet.Model.Identity.Methods;

using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of the /identity/ API category. 
    /// </summary>
    public interface IIdentityCategory
    {
        /// <summary>
        /// Trying to authorize current device in the system.
        /// </summary>
        /// <param name="params">Login paramters.</param>
        /// <param name="saveCredentials">Whether user access token should be saved in the system.</param>
        /// <returns>true if user was authorized successfully; otherwise false</returns>
        Task<bool> LoginAsync(LoginParameters @params, bool saveCredentials, CancellationToken token = default);

        /// <summary>
        /// Refreshes access token.
        /// </summary>
        Task RefreshAccessTokenAsync(CancellationToken token = default);

        /// <summary>
        /// Gets profile info.
        /// </summary>
        Task<ProfileInfoResponse> GetProfileInfoAsync(CancellationToken token = default);
    }
}
