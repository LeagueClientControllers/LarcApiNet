using System.Runtime.CompilerServices;

namespace LccApiNet.Enums.Safety
{
    /// <summary>
    /// Enumerates available device types
    /// </summary>
    public sealed class DeviceType : SafetyEnum<DeviceType>
    {
        static DeviceType()
        {
            Init();
        }

        /// <summary>
        /// Client controller
        /// </summary>
        [SafetyEnumValue("Controller")]
        public static DeviceType Controller { get; private set; } = null!;

        /// <summary>
        /// Mobile phone
        /// </summary>
        [SafetyEnumValue("Phone")]
        public static DeviceType Phone { get; private set; } = null!;
    }
}
