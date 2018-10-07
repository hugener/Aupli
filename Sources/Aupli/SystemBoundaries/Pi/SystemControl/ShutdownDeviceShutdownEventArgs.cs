// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownDeviceShutdownEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.SystemControl
{
    using Sundew.Pi.IO.Devices.PowerManagement;

    /// <summary>
    /// Implements <see cref="ShutdownEventArgs"/> for <see cref="IShutdownDevice"/>.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ShutdownDeviceShutdownEventArgs : Bridges.Shutdown.ShutdownEventArgs
    {
        private readonly Sundew.Pi.IO.Devices.PowerManagement.ShutdownEventArgs shutdownEventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownDeviceShutdownEventArgs" /> class.
        /// </summary>
        /// <param name="shutdownEventArgs">The <see cref="Sundew.Pi.IO.Devices.PowerManagement.ShutdownEventArgs" /> instance containing the event data.</param>
        public ShutdownDeviceShutdownEventArgs(ShutdownEventArgs shutdownEventArgs)
        {
            this.shutdownEventArgs = shutdownEventArgs;
        }

        /// <summary>
        /// Cancels the shutdown.
        /// </summary>
        public override void CancelShutdown()
        {
            this.shutdownEventArgs.CancelShutdown();
        }
    }
}