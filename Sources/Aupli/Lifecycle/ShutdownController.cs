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
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Logging;
    using Sundew.Pi.IO.Components.PowerManagement;

    /// <summary>
    /// Handles the shutdown process.
    /// </summary>
    public class ShutdownController
    {
        private readonly IdleController inputController;
        private readonly RemotePiConnection remotePiConnection;
        private readonly bool allowShutdown;
        private readonly CancellationTokenSource shutdownTokenSource;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownController" /> class.
        /// </summary>
        /// <param name="inputController">The input controller.</param>
        /// <param name="remotePiConnection">The shutdown button.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="shutdownTokenSource">The shutdown token source.</param>
        /// <param name="log">The log.</param>
        public ShutdownController(IdleController inputController, RemotePiConnection remotePiConnection, bool allowShutdown, CancellationTokenSource shutdownTokenSource, ILog log)
        {
            this.inputController = inputController;
            this.remotePiConnection = remotePiConnection;
            this.allowShutdown = allowShutdown;
            this.shutdownTokenSource = shutdownTokenSource;
            this.logger = log.GetCategorizedLogger(typeof(ShutdownController), true);
            this.inputController.SystemIdle += this.OnInputControllerSystemIdle;
            this.remotePiConnection.ShutdownRequested += this.OnRemotePiConnectionShutdownRequested;
        }

        /// <summary>
        /// Occurs when shutting down.
        /// </summary>
        public event EventHandler ShuttingDown;

        private void OnRemotePiConnectionShutdownRequested(object sender, EventArgs e)
        {
            this.logger.LogDebug("Remote Pi shutdown");
            this.Shutdown();
        }

        private void OnInputControllerSystemIdle(object sender, EventArgs e)
        {
            this.logger.LogDebug("System idle shutdown");
            if (this.allowShutdown)
            {
                this.remotePiConnection.Shutdown();
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