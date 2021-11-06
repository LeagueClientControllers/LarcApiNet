using System;
using System.Collections.Generic;
using System.Text;

namespace LccApi.Model
{
    /// <summary>
    /// Represents remote device that is used to control league client
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Unique identificator of the device
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Local IP address of the devices
        /// </summary>
        public string IP { get; set; }
    }
}
