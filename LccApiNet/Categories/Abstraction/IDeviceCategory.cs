using LccApiNet.Model.Device;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of the /device/ API category.
    /// </summary>
    public interface IDeviceCategory
    {
        /// <summary>
        /// Gets user's client controllers.
        /// </summary>
        Task<ReadOnlyCollection<ClientController>> GetControllersAsync(CancellationToken token = default);

        /// <summary>
        /// Gets user's client controller by its id.
        /// </summary>
        /// <param name="controllerId">Id of the requested controller.</param>
        Task<ClientController> GetControllerByIdAsync(int controllerId, CancellationToken token = default);

        /// <summary>
        /// Gets user devices.
        /// </summary>
        Task<ReadOnlyCollection<DeviceModel>> GetDevicesAsync(CancellationToken token = default);

        /// <summary>
        /// Gets user's device by its id.
        /// </summary>
        /// <param name="deviceId">Id of the requested device.</param>
        Task<DeviceModel> GetDeviceByIdAsync(int deviceId, CancellationToken token = default);
    }
}
