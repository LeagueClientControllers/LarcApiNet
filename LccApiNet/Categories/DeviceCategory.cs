using LccApiNet.Categories.Abstraction;
using LccApiNet.Model.Device;
using LccApiNet.Model.Device.Methods;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories
{
    /// <inheritdoc />
    public class DeviceCategory : IDeviceCategory
    {
        private ILccApi _api;

        public DeviceCategory(ILccApi api)
        {
            _api = api;
        }

        /// <inheritdoc />
        public async Task<ReadOnlyCollection<ClientController>> GetControllersAsync(CancellationToken token = default)
        {
            ControllersResponse response = await _api.ExecuteAsync<ControllersResponse>("/device/getControllers", true, token).ConfigureAwait(false);
            return new ReadOnlyCollection<ClientController>(response.Controllers);
        }

        /// <inheritdoc />
        public async Task<ClientController> GetControllerByIdAsync(int controllerId, CancellationToken token = default)
        {
            ControllerResponse response = await _api.ExecuteAsync<ControllerResponse, int>("/device/getControllerById", "controllerId", controllerId, true, token).ConfigureAwait(false);
            return response.Controller;
        }

        /// <inheritdoc />
        public async Task<ReadOnlyCollection<DeviceModel>> GetDevicesAsync(CancellationToken token = default)
        {
            DevicesResponse response = await _api.ExecuteAsync<DevicesResponse>("/device/getDevices", true, token).ConfigureAwait(false);
            return new ReadOnlyCollection<DeviceModel>(response.Devices);
        }

        /// <inheritdoc />
        public async Task<DeviceModel> GetDeviceByIdAsync(int deviceId, CancellationToken token = default)
        {
            DeviceResponse response = await _api.ExecuteAsync<DeviceResponse, int>("/device/getDeviceById", "deviceId", deviceId, true, token).ConfigureAwait(false);
            return response.Device;
        }
    }
}
