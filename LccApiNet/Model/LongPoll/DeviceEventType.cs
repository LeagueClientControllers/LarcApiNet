using Ardalis.SmartEnum;

namespace LccApiNet.Model.LongPoll
{
    /// <summary>
    /// Enumerates all possible types of events that can happen to the device
    /// </summary>
    internal class DeviceEventType : SmartEnum<DeviceEventType>
    {
        public DeviceEventType(string name, int value) : base(name, value) { }

        /// <summary>
        /// New device has been added
        /// </summary>
        public static DeviceEventType DeviceAdded { get; } = new DeviceEventType("DeviceAdded", 1);

        /// <summary>
        /// One of the device's properties has been changed
        /// </summary>
        public static DeviceEventType DeviceChanged { get; } = new DeviceEventType("DeviceChanged", 2);

        /// <summary>
        /// Device has been removed
        /// </summary>
        public static DeviceEventType DeviceRemoved { get; } = new DeviceEventType("DeviceRemoved", 3);
    }
}
