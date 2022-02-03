using LccApiNet.Model.Identity;
using LccApiNet.Model.Identity.Methods;

using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of the /identity/ API category. 
    /// </summary>
    public interface IIdentityCategory
    {
        /// <summary>
        /// Trying to authorize current device in the system.
        /// </summary>
        /// <param name="params">Login parameters.</param>
        /// <param name="saveCredentials">Whether user access token should be saved in the system.</param>
        /// <returns>true if user was authorized successfully; otherwise false</returns>
        Task<bool> LoginAsync(LoginParameters @params, bool saveCredentials = false, CancellationToken token = default);

        /// <summary>
        /// Refreshes access token.
        /// </summary>
        Task RefreshAccessTokenAsync(CancellationToken token = default);

        /// <summary>
        /// Gets profile info.
        /// </summary>
        Task<ProfileInfo> GetProfileInfoAsync(CancellationToken token = default);
    }
}
