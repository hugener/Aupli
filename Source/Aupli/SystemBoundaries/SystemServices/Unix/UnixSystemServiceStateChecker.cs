// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnixSystemServiceStateChecker.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Unix
{
    using System.Diagnostics;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;

    /// <summary>
    /// Uses systemctl to check whether the specified services are running.
    /// </summary>
    /// <seealso cref="ISystemServicesAwaiter" />
    public class UnixSystemServiceStateChecker : ISystemServiceStateChecker
    {
        /// <summary>
        /// Checks if the specified service is running.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>a value indicating whether the specified service is running.</returns>
        public bool IsServiceRunning(string serviceName)
        {
            var processStartInfo = new ProcessStartInfo("systemctl", $"is-active --quiet {serviceName}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };

            using (var process = Process.Start(processStartInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}