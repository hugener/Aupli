// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Shutdown
{
    using System;
    using System.Threading;
    using Aupli.SystemBoundaries.Shared.System;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Handles the shutdown process.
    /// </summary>
    public class ShutdownController : IShutdownNotifier
    {
        private readonly IdleController inputController;
        private readonly ISystemControl systemControl;
        private readonly bool allowShutdown;
        private readonly CancellationTokenSource shutdownTokenSource;
        private readonly IShutdownControllerReporter shutdownControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownController" /> class.
        /// </summary>
        /// <param name="inputController">The input controller.</param>
        /// <param name="systemControl">The shutdown button.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="shutdownTokenSource">The shutdown token source.</param>
        /// <param name="shutdownControllerReporter">The shutdown controller reporter.</param>
        public ShutdownController(
            IdleController inputController,
            ISystemControl systemControl,
            bool allowShutdown,
            CancellationTokenSource shutdownTokenSource,
            IShutdownControllerReporter shutdownControllerReporter = null)
        {
            this.inputController = inputController;
            this.systemControl = systemControl;
            this.allowShutdown = allowShutdown;
            this.shutdownTokenSource = shutdownTokenSource;
            this.shutdownControllerReporter = shutdownControllerReporter;
            this.shutdownControllerReporter?.SetSource(this);
            this.inputController.SystemIdle += this.OnInputControllerSystemIdle;
            this.systemControl.ShuttingDown += this.OnShutdownControlShuttingDown;
        }

        /// <summary>
        /// Occurs when shutting down.
        /// </summary>
        public event EventHandler ShuttingDown;

        private void OnShutdownControlShuttingDown(object sender, ShutdownEventArgs e)
        {
            this.shutdownControllerReporter?.RemotePiShutdown();
            if (this.allowShutdown)
            {
                this.shutdownControllerReporter?.ShuttingDownAupli();
            }
            else
            {
                e.CancelShutdown();
                this.shutdownControllerReporter?.ClosingAupli();
            }

            this.Shutdown();
        }

        private void OnInputControllerSystemIdle(object sender, EventArgs e)
        {
            this.shutdownControllerReporter?.SystemIdleShutdown();
            if (this.allowShutdown)
            {
                this.systemControl.Shutdown();
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