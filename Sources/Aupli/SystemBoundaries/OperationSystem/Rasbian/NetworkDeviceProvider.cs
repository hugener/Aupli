// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkDeviceProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.OperationSystem.Rasbian
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using Aupli.SystemBoundaries.Connectors.System;

    /// <summary>
    /// Provides the IP address on a Unix system.
    /// </summary>
    public class NetworkDeviceProvider : INetworkDeviceProvider
    {
        /// <summary>
        /// Tries the get network devices.
        /// </summary>
        public IEnumerable<NetworkDevice> GetNetworkDevices()
        {
            var processStartInfo =
                new ProcessStartInfo("ifconfig")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                };

            var ifConfigProcess = Process.Start(processStartInfo);
            var networkDevices = new List<NetworkDevice>();
            var networkDeviceName = "Unknown";
            while (!ifConfigProcess.StandardOutput.EndOfStream)
            {
                var line = ifConfigProcess.StandardOutput.ReadLine();
                if (line == null)
                {
                    continue;
                }

                if (!line.StartsWith(" "))
                {
                    var endOfDeviceNameIndex = line.IndexOf(": ", StringComparison.InvariantCulture);
                    if (endOfDeviceNameIndex > 0)
                    {
                        networkDeviceName = line.Substring(0, endOfDeviceNameIndex);
                    }
                }

                if (line.Contains("inet "))
                {
                    var ip = line.Substring(13, 15);
                    var firstSpace = ip.IndexOf(' ');
                    if (firstSpace != -1)
                    {
                        ip = ip.Substring(0, firstSpace);
                    }

                    if (IPAddress.TryParse(ip, out var ipAddress))
                    {
                        networkDevices.Add(new NetworkDevice(networkDeviceName, ipAddress));
                        networkDeviceName = "Unknown";
                    }
                }
            }

            ifConfigProcess.WaitForExit();
            return networkDevices;
        }
    }
}
