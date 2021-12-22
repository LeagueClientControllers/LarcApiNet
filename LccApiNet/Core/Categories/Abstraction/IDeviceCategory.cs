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
    /// Contains methods of the /device/ API category 
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
