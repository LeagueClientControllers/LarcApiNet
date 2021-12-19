using LccApiNet.Model.Device;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories.Abstraction
{
    /// <summary>
    /// Abstraction of the API category that contains methods related to user identification.
    /// </summary>
    public interface IDeviceCategory
    {
        /// <summary>
        /// Gets user devices
        /// </summary>
        /// <returns>Collection of the devices owned by the user</returns>
        Task<ReadOnlyCollection<Device>> GetDevices(CancellationToken token = default);
    }
}
