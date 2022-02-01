using LccApiNet.Services;

namespace LccApiNet.EventHandlers
{
    /// <summary>
    /// Represents a function that will handle <see cref="UserEventService.DeviceAdded"/> event.
    /// </summary>
    public delegate void DeviceAddedEventHandler(object sender, DeviceAddedEventArgs args);

    /// <summary>
    /// Provides data for <see cref="UserEventService.DeviceAdded"/>.
    /// </summary>
    public class DeviceAddedEventArgs
    {
        /// <summary>
        /// Id of the device that has been added.
        /// </summary>
        public int DeviceId;

        public DeviceAddedEventArgs(int deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
