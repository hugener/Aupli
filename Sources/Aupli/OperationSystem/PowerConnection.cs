// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PowerConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.OperationSystem
{
    using System.Diagnostics;

    /// <summary>
    /// Connection to power functionality.
    /// </summary>
    public class PowerConnection
    {
        /// <summary>
        /// Executes and shutdown now command.
        /// </summary>
        public void Shutdown()
        {
            Process.Start(new ProcessStartInfo("sudo", "shutdown -h now"));
        }
    }
}
