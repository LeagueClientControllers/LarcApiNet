using Ardalis.SmartEnum;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Enumerates all possible types of events that can happen to the device
    /// </summary>
    public class DeviceEventType : SmartEnum<DeviceEventType>
    {
        public DeviceEventType(string name, int value) : base(name, value) { }

        /// <summary>
        /// New device has been added
        /// </summary>
        public static readonly DeviceEventType DeviceAdded = new DeviceEventType("DeviceAdded", 1);

        /// <summary>
        /// One of the device's properties has been changed
        /// </summary>
        public static readonly DeviceEventType DeviceChanged = new DeviceEventType("DeviceChanged", 2);

        /// <summary>
        /// Device has been removed
        /// </summary>
        public static readonly DeviceEventType DeviceRemoved = new DeviceEventType("DeviceRemoved", 3);
    }
}
