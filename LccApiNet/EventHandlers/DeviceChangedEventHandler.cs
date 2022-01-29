using System.Collections.Generic;

namespace LccApiNet.EventHandlers
{
    /// <summary>
    /// Represents a function that will handle <see cref="Core.Services.UserEventService.DeviceChanged"/> event.
    /// </summary>
    public delegate void DeviceChangedEventHandler(object sender, DeviceChangedEventArgs args);

    /// <summary>
    /// Provides data for the <see cref="Core.Services.UserEventService.DeviceChanged"/> event.
    /// </summary>
    public class DeviceChangedEventArgs
    {
        /// <summary>
        /// Id of the device that has been changed.
        /// </summary>
        public int DeviceId { get; }

        /// <summary>
        /// Dictionary with property names and their new values.
        /// </summary>
        public Dictionary<string, object> Changes { get; }

        public DeviceChangedEventArgs(int deviceId, Dictionary<string, object> changes)
        {
            DeviceId = deviceId;
            Changes = changes;
        }
    }
}
