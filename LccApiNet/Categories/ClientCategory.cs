using LccApiNet.Categories.Abstraction;
using LccApiNet.Model.Client;
using LccApiNet.Model.Client.Commands;
using LccApiNet.Model.Client.Methods;
using LccApiNet.Model.Device;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories
{
    /// <inheritdoc />
    public class ClientCategory : IClientCategory
    {
        private ILccApi _api;

        public ClientCategory(ILccApi api)
        {
            _api = api;
        }

        /// <inheritdoc />
        public async Task SetGameflowPhaseAsync(GameflowPhase? gameflowPhase, CancellationToken token = default) => 
            await _api.ExecuteAsync("/client/setGameflowPhase", "gameflowPhase", gameflowPhase?.Name, true, token).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<int> SendCommandAsync(int controllerId, CommandName commandName, CancellationToken token = default) => 
            await _api.ExecuteAsync<int, SendCommandParameters>("/client/sendCommand", new SendCommandParameters(controllerId, commandName), "commandId", true, token).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task SetCommandResultAsync(int commandId, CommandResult result, CancellationToken token = default) => 
            await _api.ExecuteAsync("/client/setCommandResult", new SetCommandResultParameters(commandId, result), true, token).ConfigureAwait(false);
    }
}
