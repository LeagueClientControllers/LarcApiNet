using LccApiNet.Categories.Abstraction;
using LccApiNet.Model.Client;
using LccApiNet.Model.Client.Commands;
using LccApiNet.Model.Client.Methods;

using System;
using System.Runtime.InteropServices;
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
        public async Task SetGameflowPhaseAsync(GameflowPhase? gameflowPhase, DateTime? readyCheckStarted = null, CancellationToken token = default)
        {
            if (gameflowPhase == GameflowPhase.ReadyCheck) {
                if (readyCheckStarted == null) {
                    throw new ArgumentException("When game flow phase is ready check, ready check started is required.", "readyCheckStarted");
                }

                DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                await _api.ExecuteAsync("/client/setGameflowPhase", new SetGameflowPhaseParameters(gameflowPhase, (long)(readyCheckStarted - sTime).Value.TotalSeconds), true, token).ConfigureAwait(false);
            } else {
                await _api.ExecuteAsync("/client/setGameflowPhase", new SetGameflowPhaseParameters(gameflowPhase), true, token).ConfigureAwait(false);
            }
        } 

        /// <inheritdoc />
        public async Task<int> SendCommandAsync(int controllerId, CommandName commandName, CancellationToken token = default)
        {
            return await _api.ExecuteAsync<int, SendCommandParameters>("/client/sendCommand", new SendCommandParameters(controllerId, commandName), "commandId", true, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SetCommandResultAsync(int commandId, CommandResult result, CancellationToken token = default)
        {
            await _api.ExecuteAsync("/client/setCommandResult", new SetCommandResultParameters(commandId, result), true, token).ConfigureAwait(false);
        } 
    }
}
