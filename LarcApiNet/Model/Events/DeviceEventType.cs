#nullable enable
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ardalis.SmartEnum;


namespace LarcApiNet.Model {
    
    
    /// <summary>
    /// Type of the device event
    /// </summary>
    public class DeviceEventType : SmartEnum<DeviceEventType> {
        
        /// <summary>
        /// Device was added
        /// </summary>
        public static DeviceEventType DeviceAdded = new DeviceEventType("DeviceAdded", 1);
        
        /// <summary>
        /// Some of device properties were changed
        /// </summary>
        public static DeviceEventType DeviceChanged = new DeviceEventType("DeviceChanged", 2);
        
        /// <summary>
        /// Device was removed
        /// </summary>
        public static DeviceEventType DeviceRemoved = new DeviceEventType("DeviceRemoved", 3);
        
        public DeviceEventType(string name, int value) : 
                base(name, value) {
        }
    }
}

#nullable restore
