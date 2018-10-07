// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkDeviceInfoProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Internal.Net
{
    using System.Collections.Generic;
    using System.Net.NetworkInformation;
    using Aupli.SystemBoundaries.UserInterface.Menu.Ari;

    /// <summary>
    /// Provides the IP address on a Unix system.
    /// </summary>
    public class NetworkDeviceInfoProvider : INetworkDeviceInfoProvider
    {
        /// <summary>
        /// Gets the network device infos.
        /// </summary>
        /// <returns>The network device infos.</returns>
        public IEnumerable<NetworkDeviceInfo> GetNetworkDeviceInfos()
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                var properties = networkInterface.GetIPProperties();
                foreach (var ipAddressInformation in properties.UnicastAddresses)
                {
                    if (ipAddressInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        yield return new NetworkDeviceInfo(networkInterface.Name, ipAddressInformation.Address);
                    }
                }
            }
        }
    }
}
