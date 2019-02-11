// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsSystemServiceStateChecker.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Windows
{
    using System.Diagnostics;
    using Aupli.SystemBoundaries.SystemServices.Ari;

    /// <summary>
    /// Check whether a service is running on windows.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.SystemServices.Ari.ISystemServiceStateChecker" />
    public class WindowsSystemServiceStateChecker : ISystemServiceStateChecker
    {
        /// <summary>
        /// Checks if the specified service is running.
        /// </summary>
        /// <param name="serviceName">The service name.</param>
        /// <returns>a value indicating whether the specified service is running.</returns>
        public bool IsServiceRunning(string serviceName)
        {
            var processStartInfo = new ProcessStartInfo("cmd", $"/c sc query {serviceName} | findstr RUNNING")
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