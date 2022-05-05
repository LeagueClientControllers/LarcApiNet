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
    /// Contains methods that are used to control the league game client.
    /// </summary>
    public interface IClientCategory {
        
        /// <summary>
        /// Sets current game flow phase of the league client.
        /// </summary>
        /// <param name="gameflowPhase">Current league client game flow phase to set.</param>
        /// <param name="readyCheckStarted">If game flow phase is ready check, this property determines timestamp when ready check was started in unix format.</param>
        [ControllerOnly()]
        Task SetGameflowPhaseAsync(GameflowPhase? gameflowPhase, int? readyCheckStarted, CancellationToken token = default);
        
        /// <summary>
        /// Sends command to a client controller that is specified in the parameters to execute.
        /// </summary>
        /// <param name="controllerId">Determine which controller should execute this command.</param>
        /// <param name="commandName">Command that should be sent to the client controller.</param>
        /// <param name="commandArgs">Arguments of the command.</param>
        [DeviceOnly()]
        Task<int> SendCommandAsync(int controllerId, CommandName commandName, SomeParametrizedCommandArgs? commandArgs, CancellationToken token = default);
    }
}

#nullable restore
