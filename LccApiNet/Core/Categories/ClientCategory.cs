using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.Client.Methods;
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
        public async Task SetGameflowPhase(SetGameflowPhaseParameters @params, CancellationToken token = default)
        {
            await _api.ExecuteAsync("/client/setGameflowPhase", @params, true, token);
        }
    }
}
