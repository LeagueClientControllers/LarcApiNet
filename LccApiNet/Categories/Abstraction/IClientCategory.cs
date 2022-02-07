using LccApiNet.Model.Client;
using LccApiNet.Model.Client.Commands;
using LccApiNet.Model.Device;

using System;
using System.Collections.Generic;
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
        Task SetGameflowPhaseAsync(GameflowPhase? gameflowPhase, DateTime? readyCheckStarted = null, CancellationToken token = default);

        /// <summary>
        /// Sends command to be executed on the client controller.
        /// </summary>
        /// <param name="controllerId">Id of the controller the command destined for</param>
        /// <param name="commandName">Name of the command that determines it's type</param>
        Task<int> SendCommandAsync(int controllerId, CommandName commandName, CancellationToken token = default); 

        /// <summary>
        /// Sets result of the command after execution.
        /// </summary>
        /// <param name="commandId">Id of the command.</param>
        /// <param name="result">Result of the command.</param>
        Task SetCommandResultAsync(int commandId, CommandResult result, CancellationToken token = default);
    }
}
