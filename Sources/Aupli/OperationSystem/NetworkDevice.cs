// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkDevice.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.OperationSystem
{
    using System.Net;

    /// <summary>
    /// Represents a network device.
    /// </summary>
    public class NetworkDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkDevice"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ipAddress">The ip address.</param>
        public NetworkDevice(string name, IPAddress ipAddress)
        {
            this.Name = name;
            this.IpAddress = ipAddress;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <value>
        /// The ip address.
        /// </value>
        public IPAddress IpAddress { get; }
    }
}