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
        public async Task<ReadOnlyCollection<DeviceModel>> GetDevicesAsync(CancellationToken token = default)
        {
            DevicesResponse response = await _api.ExecuteAsync<DevicesResponse>("/device/getDevices", true, token);
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
