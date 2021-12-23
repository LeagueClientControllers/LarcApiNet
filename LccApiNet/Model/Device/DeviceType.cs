using Ardalis.SmartEnum;

namespace LccApiNet.Model.Device
{
    /// <summary>
    /// Enumerates available device types
    /// </summary>
    public sealed class DeviceType : SmartEnum<DeviceType>
    {
        public DeviceType(string name, int value) : base(name, value) {}

        /// <summary>
        /// Client controller
        /// </summary>
        public static readonly DeviceType Controller = new DeviceType("Controller", 1);

        /// <summary>
        /// Mobile phone
        /// </summary>
        public static readonly DeviceType Phone = new DeviceType("Phone", 2);
    }
}
