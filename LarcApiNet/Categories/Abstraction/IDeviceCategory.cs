#nullable enable
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LarcApiNet.Exceptions;
using LarcApiNet.Model;
using NetLibraryGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace LarcApiNet.Categories.Abstraction {
    
    
    /// <summary>
    /// Contains methods that are used to retrieve or update information about user's devices.
    /// </summary>
    public interface IDeviceCategory {
        
        /// <summary>
        /// Returns all the devices that are registered
        /// to the current user.
        /// </summary>
        Task<List<Device>> GetDevicesAsync(CancellationToken token = default);
        
        /// <summary>
        /// Fetches information about the device by its id.
        /// </summary>
        /// <param name="id"></param>
        [ControllerOnly()]
        Task<Device> GetDeviceByIdAsync(int id, CancellationToken token = default);
        
        /// <summary>
        /// Changes the name of the device.
        /// </summary>
        /// <param name="name"></param>
        [DeviceOnly()]
        Task ChangeDeviceNameAsync(string name, CancellationToken token = default);
        
        /// <summary>
        /// Gets all the client controller that are registered
        /// to the current user.
        /// </summary>
        [DeviceOnly()]
        Task<List<ClientController>> GetControllersAsync(CancellationToken token = default);
        
        /// <summary>
        /// Fetches information about the controller by its id.
        /// </summary>
        /// <param name="id"></param>
        [DeviceOnly()]
        Task<ClientController> GetControllerByIdAsync(int id, CancellationToken token = default);
        
        /// <summary>
        /// Changes the name of the controller.
        /// </summary>
        /// <param name="name"></param>
        [ControllerOnly()]
        Task ChangeControllerNameAsync(string name, CancellationToken token = default);
    }
}

#nullable restore
