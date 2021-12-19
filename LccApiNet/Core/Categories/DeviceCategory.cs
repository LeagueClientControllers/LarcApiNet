using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.Device;
using LccApiNet.Model.Device.Methods;

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories
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
        public async Task<ReadOnlyCollection<Device>> GetDevices(CancellationToken token = default)
        {
            GetDevicesResponse response = await _api!.ExecuteAsync<GetDevicesResponse>("/device/getDevices", true, token);
            return new ReadOnlyCollection<Device>(response.Devices);
        }
    }
}
