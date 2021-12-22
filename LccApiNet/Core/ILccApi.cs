using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.General;

using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core
{
    /// <summary>
    /// Abstraction of the main API class
    /// </summary>
    public interface ILccApi
    {
        /// <summary>
        /// Payload of the <see cref="AccessToken"/>
        /// </summary>
        JwtPayload? AccessTokenContent { get; }

        /// <summary>
        /// JWT that is used to get access to the user methods
        /// </summary>
        string? AccessToken { get; }

        /// <summary>
        /// API Identity category
        /// </summary>
        IIdentityCategory Identity { get; }

        /// <summary>
        /// API Device category
        /// </summary>
        IDeviceCategory Device { get; }

        /// <summary>
        /// Initializes the API module. 
        /// Tries to get stored access token and refreshes it
        /// </summary>
        Task InitAsync(CancellationToken token = default);

        /// <summary>
        /// Executes API method with parameters and a response
        /// </summary>
        /// <typeparam name="TResponse">Type of the response model</typeparam>
        /// <typeparam name="TParameters">Type of the parameters model</typeparam>
        /// <param name="methodPath">Path to the method</param>
        /// <param name="params">Method parameters</param>
        /// <param name="withAccessToken">Whether access token should be provided to execute method</param>
        /// <returns>Method response</returns>
        Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse;

        /// <summary>
        /// Executes API method with a response without parameters
        /// </summary>
        /// <typeparam name="TResponse">Type of the response model</typeparam>
        /// <param name="methodPath">Path to the method</param>
        /// <param name="withAccessToken">Whether access token should be provided to execute method</param>
        /// <returns>Method response</returns>
        Task<TResponse> ExecuteAsync<TResponse>(string methodPath, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse;

        /// <summary>
        /// Executes API method with parameters and without response
        /// </summary>
        /// <typeparam name="TParameters">Type of the parameters model</typeparam>
        /// <param name="methodPath">Path to the method</param>
        /// <param name="params">Method parameters</param>
        /// <param name="withAccessToken">Whether access token should be provided to execute method</param>
        Task ExecuteAsync<TParameters>(string methodPath, TParameters @params, bool withAccessToken = true, CancellationToken token = default);
        

        /// <summary>
        /// Updates access token that used to execute user methods
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="storeInSystem">Whether access token should be stored in the system</param>
        Task UpdateAccessToken(string accessToken, bool storeInSystem = false);
    }
}
