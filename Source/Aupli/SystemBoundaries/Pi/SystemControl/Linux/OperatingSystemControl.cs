﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperatingSystemControl.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.SystemControl.Linux
{
    using System.Diagnostics;
    using Sundew.Pi.IO.Devices.PowerManagement;

    /// <summary>
    /// Connection to power functionality.
    /// </summary>
    public class OperatingSystemControl : IOperatingSystemShutdown
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
