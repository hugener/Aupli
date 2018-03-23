// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Lifecycle
{
    using System;
    using System.Threading;
    using Serilog;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.IO.Devices.PowerManagement;

    /// <summary>
    /// Handles the shutdown process.
    /// </summary>
    public class ShutdownController
    {
        private readonly IdleController inputController;
        private readonly RemotePiDevice remotePiDevice;
        private readonly bool allowShutdown;
        private readonly CancellationTokenSource shutdownTokenSource;
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownController" /> class.
        /// </summary>
        /// <param name="inputController">The input controller.</param>
        /// <param name="remotePiDevice">The shutdown button.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="shutdownTokenSource">The shutdown token source.</param>
        /// <param name="logger">The logger.</param>
        public ShutdownController(
            IdleController inputController,
            RemotePiDevice remotePiDevice,
            bool allowShutdown,
            CancellationTokenSource shutdownTokenSource,
            ILogger logger)
        {
            this.inputController = inputController;
            this.remotePiDevice = remotePiDevice;
            this.allowShutdown = allowShutdown;
            this.shutdownTokenSource = shutdownTokenSource;
            this.log = logger.ForContext<ShutdownController>();
            this.inputController.SystemIdle += this.OnInputControllerSystemIdle;
            this.remotePiDevice.ShuttingDown += this.OnRemotePiDeviceShuttingDown;
        }

        /// <summary>
        /// Occurs when shutting down.
        /// </summary>
        public event EventHandler ShuttingDown;

        private void OnRemotePiDeviceShuttingDown(object sender, ShutdownEventArgs e)
        {
            this.log.Debug("Remote Pi shutdown");
            if (this.allowShutdown)
            {
                this.log.Information("Shutting down Aupli");
            }
            else
            {
                e.CancelShutdown();
                this.log.Information("Closing Aupli");
            }

            this.Shutdown();
        }

        private void OnInputControllerSystemIdle(object sender, EventArgs e)
        {
            this.log.Debug("System idle shutdown");
            if (this.allowShutdown)
            {
                this.remotePiDevice.Shutdown();
            }
            else
            {
                this.Shutdown();
            }
        }

        private void Shutdown()
        {
            this.ShuttingDown?.Invoke(this, EventArgs.Empty);
            this.shutdownTokenSource.Cancel();
        }
    }
}