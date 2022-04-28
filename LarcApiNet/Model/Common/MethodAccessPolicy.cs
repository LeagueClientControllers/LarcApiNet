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


namespace LccApiNet.Model {
    
    
    /// <summary>
    /// Determines who can execute specific method
    /// </summary>
    public class MethodAccessPolicy : SmartEnum<MethodAccessPolicy> {
        
        /// <summary>
        /// Method can be accessed with token given to the client controller
        /// </summary>
        public static MethodAccessPolicy Controller = new MethodAccessPolicy("Controller", 1);
        
        /// <summary>
        /// Method can be accessed with token given to the remote device
        /// </summary>
        public static MethodAccessPolicy Device = new MethodAccessPolicy("Device", 2);
        
        /// <summary>
        /// Method can be accessed with any access token
        /// </summary>
        public static MethodAccessPolicy Both = new MethodAccessPolicy("Both", 3);
        
        public MethodAccessPolicy(string name, int value) : 
                base(name, value) {
        }
    }
}

#nullable restore