using System;
using System.Net.Sockets;

namespace LarcApiNet.Exceptions
{
    /// <summary>
    /// The exception that is thrown when <see cref="SocketException"/> exception occurred with <see cref="SocketError.HostUnreachable"/> code
    /// that means that the device is not connected to the Internet.
    /// </summary>
    public class NetworkUnreachableException : Exception
    {
        public NetworkUnreachableException(): base("Attempted to execute method without being connected to the Internet.") { }
    }
}
