// <copyright file="ShutdownEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Sundew.Pi.IO.Components.PowerManagement
{
    using System;
    using System.Threading;

    /// <summary>
    /// Event args for the shutting down event.
    /// </summary>
    public class ShutdownEventArgs : EventArgs
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownEventArgs" /> class.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancellation token.</param>
        /// <param name="shutdownStartTime">The shutdown start time.</param>
        public ShutdownEventArgs(
            CancellationTokenSource cancellationTokenSource,
            DateTime shutdownStartTime)
        {
            this.cancellationTokenSource = cancellationTokenSource;
            this.ShutdownStartTime = shutdownStartTime;
            this.ShutdownTime = new DateTimeOffset(this.ShutdownStartTime, RemotePiConnection.ShutdownTimeSpan);
            this.PowerOffTime = new DateTimeOffset(this.ShutdownStartTime, RemotePiConnection.PowerOffTimeSpan);
        }

        /// <summary>
        /// Gets the shutdown start time.
        /// </summary>
        /// <value>
        /// The shutdown start time.
        /// </value>
        public DateTime ShutdownStartTime { get; }

        /// <summary>
        /// Gets the time until shutdown.
        /// </summary>
        /// <value>
        /// The time until shutdown.
        /// </value>
        public DateTimeOffset ShutdownTime { get; }

        /// <summary>
        /// Gets the time until power off.
        /// </summary>
        /// <value>
        /// The time until power off.
        /// </value>
        public DateTimeOffset PowerOffTime { get; }

        /// <summary>
        /// Cancels the shutdown.
        /// </summary>
        public void CancelShutdown()
        {
            this.cancellationTokenSource.Cancel();
        }
    }
}