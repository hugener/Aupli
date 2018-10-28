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
    using Aupli.SystemBoundaries.Bridges.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Shutdown.Api;
    using Aupli.SystemBoundaries.UserInterface.Shutdown.Ari;
    using Sundew.Pi.ApplicationFramework;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Handles the shutdown process.
    /// </summary>
    public class ShutdownController : IShutdownNotifier
    {
        private readonly IIdleMonitor idleMonitor;
        private readonly ISystemControl systemControl;
        private readonly IApplicationExit applicationExit;
        private readonly bool allowShutdown;
        private readonly CancellationTokenSource shutdownTokenSource;
        private readonly IShutdownControllerReporter shutdownControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownController" /> class.
        /// </summary>
        /// <param name="idleMonitor">The input controller.</param>
        /// <param name="systemControl">The shutdown button.</param>
        /// <param name="applicationExit">The application exit.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="shutdownTokenSource">The shutdown token source.</param>
        /// <param name="shutdownControllerReporter">The shutdown controller reporter.</param>
        public ShutdownController(
            IIdleMonitor idleMonitor,
            ISystemControl systemControl,
            IApplicationExit applicationExit,
            bool allowShutdown,
            CancellationTokenSource shutdownTokenSource,
            IShutdownControllerReporter shutdownControllerReporter = null)
        {
            this.idleMonitor = idleMonitor;
            this.systemControl = systemControl;
            this.applicationExit = applicationExit;
            this.allowShutdown = allowShutdown;
            this.shutdownTokenSource = shutdownTokenSource;
            this.shutdownControllerReporter = shutdownControllerReporter;
            this.shutdownControllerReporter?.SetSource(this);
            this.idleMonitor.SystemIdle += this.OnIdleControllerSystemIdle;
            this.systemControl.ShuttingDown += this.OnShutdownControlShuttingDown;
            this.applicationExit.ExitRequest += this.OnApplicationExitRequest;
            this.applicationExit.Exiting += this.OnApplicationExiting;
            /*Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                this.shutdownControllerReporter?.ShutdownByCtrlC();
                this.Shutdown();
            };*/
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

            this.applicationExit.Exit();
        }

        private void OnIdleControllerSystemIdle(object sender, EventArgs e)
        {
            this.shutdownControllerReporter?.SystemIdleShutdown();
            if (this.allowShutdown)
            {
                this.systemControl.Shutdown();
            }
            else
            {
                this.applicationExit.Exit();
            }
        }

        private void OnApplicationExiting(object sender, EventArgs e)
        {
            this.ShuttingDown?.Invoke(this, EventArgs.Empty);
        }

        private void OnApplicationExitRequest(object sender, ExitRequestEventArgs e)
        {
            if (this.allowShutdown)
            {
                this.systemControl.Shutdown();
                e.Cancel = true;
            }
        }
    }
}