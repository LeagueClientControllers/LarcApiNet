using LccApiNet.Model.Client.Methods;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of /client/ API category.
    /// </summary>
    public interface IClientCategory
    {
        /// <summary>
        /// Sets current game flow phase of the league client.
        /// </summary>
        Task SetGameflowPhase(SetGameflowPhaseParameters @params, CancellationToken token = default);
    }
}
