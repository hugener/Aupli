// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownSystemControl.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.SystemControl
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Shutdown;
    using Sundew.Pi.IO.Devices.PowerManagement;

    /// <summary>
    /// Implements <see cref="ISystemControl"/> using a <see cref="IShutdownDevice"/>.
    /// </summary>
    /// <seealso cref="ISystemControl" />
    public class ShutdownSystemControl : ISystemControl
    {
        private readonly IShutdownDevice shutdownDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownSystemControl"/> class.
        /// </summary>
        /// <param name="shutdownDevice">The remote pi device.</param>
        public ShutdownSystemControl(IShutdownDevice shutdownDevice)
        {
            this.shutdownDevice = shutdownDevice;
            this.shutdownDevice.ShuttingDown += (sender, args) => this.ShuttingDown?.Invoke(this, new ShutdownDeviceShutdownEventArgs(args));
        }

        /// <summary>
        /// Occurs when [shutting down].
        /// </summary>
        public event EventHandler<Bridges.Shutdown.ShutdownEventArgs> ShuttingDown;

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            this.shutdownDevice.Shutdown();
        }
    }
}