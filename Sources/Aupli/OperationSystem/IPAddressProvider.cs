// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPAddressProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.OperationSystem
{
    using System.Diagnostics;
    using System.Net;

    /// <summary>
    /// Provides the IP address on a Unix system.
    /// </summary>
    public class IPAddressProvider
    {
        /// <summary>
        /// Tries the get system ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns><c>true</c>, if the system address could be obtained, otherwise <c>false</c>.</returns>
        public bool TryGetSystemIpAddress(out IPAddress ipAddress)
        {
            var processStartInfo =
                new ProcessStartInfo($"ifconfig", "eth0")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                };

            var ifConfigProcess = Process.Start(processStartInfo);
            ipAddress = IPAddress.None;
            while (!ifConfigProcess.StandardOutput.EndOfStream)
            {
                var line = ifConfigProcess.StandardOutput.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (line.Contains("inet "))
                {
                    var ip = line.Substring(13, 15);
                    var firstSpace = ip.IndexOf(' ');
                    if (firstSpace != -1)
                    {
                        ip = ip.Substring(0, firstSpace);
                    }

                    if (!IPAddress.TryParse(ip, out ipAddress))
                    {
                        ipAddress = IPAddress.None;
                    }

                    break;
                }
            }

            ifConfigProcess.WaitForExit();
            return !Equals(ipAddress, IPAddress.None);
        }
    }
}
