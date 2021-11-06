using LccApiNet.Core.Categories.Abstraction;

using System.Threading.Tasks;

namespace LccApi.Core
{
    /// <summary>
    /// Abstraction of the main API class
    /// </summary>
    public interface ILccApi
    {
        /// <summary>
        /// JWT that is used to get access to the user methods
        /// </summary>
        string? AccessToken { get; }

        /// <summary>
        /// API Identity category
        /// </summary>
        IIdentityCategory Identity { get; }

        /// <summary>
        /// Executes API method with parameters and response
        /// </summary>
        /// <typeparam name="TResponse">Type of the response model</typeparam>
        /// <typeparam name="TParameters">Type of the parameters model</typeparam>
        /// <param name="methodPath">Path to the method</param>
        /// <param name="params">Method parameters</param>
        /// <param name="withAccessToken">Whether access token should be provided to execute method</param>
        /// <returns>Method response</returns>
        Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, bool withAccessToken = true);
    }
}
