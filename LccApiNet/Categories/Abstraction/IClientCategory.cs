using LccApiNet.Model.Client;
using LccApiNet.Model.Client.Commands;
using LccApiNet.Model.Client.Methods;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of /client/ API category.
    /// </summary>
    public interface IClientCategory
    {
        /// <summary>
        /// Sets current game flow phase of the league client.
        /// </summary>
        Task SetGameflowPhase(GameflowPhase? gameflowPhase, CancellationToken token = default);

        /// <summary>
        /// Sets result of the command after execution.
        /// </summary>
        /// <param name="commandId">Id of the command.</param>
        /// <param name="result">Result of the command.</param>
        Task SetCommandResult(int commandId, CommandResult result, CancellationToken token = default);

    }
}
