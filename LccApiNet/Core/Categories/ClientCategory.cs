using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.Client;
using LccApiNet.Model.Client.Commands;
using LccApiNet.Model.Client.Methods;

using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories
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
        public async Task SetGameflowPhase(GameflowPhase? gameflowPhase, CancellationToken token = default)
        {
            await _api.ExecuteAsync<string?>("/client/setGameflowPhase", "gameflowPhase", gameflowPhase?.Name, true, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task SetCommandResult(int commandId, CommandResult result, CancellationToken token = default)
        {
            await _api.ExecuteAsync<SetCommandResultParameters>("/client/setCommandResult", new SetCommandResultParameters(commandId, result), true, token).ConfigureAwait(false);
        }
    }
}
