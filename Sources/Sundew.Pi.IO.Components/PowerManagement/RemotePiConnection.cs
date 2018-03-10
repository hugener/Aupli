// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemotePiConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.PowerManagement
{
    using System;
    using global::Pi.IO.GeneralPurpose;
    using global::Pi.System.Threading;

    /// <summary>
    /// Represents a connection to the RemotePI for shutting down PI.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RemotePiConnection : IDisposable
    {
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly ConnectorPin shutdownInConnectorPin;
        private readonly ConnectorPin shutdownOutConnectorPin;
        private readonly IThread thread;
        private readonly GpioConnection gpioConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePiConnection" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="shutdownInConnectorPin">The shutdown in connector pin.</param>
        /// <param name="shutdownOutConnectorPin">The shutdown out connector pin.</param>
        /// <param name="threadFactory">The thread factory.</param>
        public RemotePiConnection(
            IGpioConnectionDriver gpioConnectionDriver,
            ConnectorPin shutdownInConnectorPin,
            ConnectorPin shutdownOutConnectorPin,
            IThreadFactory threadFactory = null)
        {
            this.gpioConnectionDriver = gpioConnectionDriver;
            this.shutdownInConnectorPin = shutdownInConnectorPin;
            this.shutdownOutConnectorPin = shutdownOutConnectorPin;
            this.thread = ThreadFactory.EnsureThreadFactory(threadFactory).Create();
            var pinConfiguration = shutdownInConnectorPin.Input().PullDown();
            pinConfiguration.OnStatusChanged(this.OnShutdown);
            this.gpioConnection = new GpioConnection(pinConfiguration);
        }

        /// <summary>
        /// Occurs when remote pi requests system shutdown.
        /// </summary>
        public event EventHandler ShutdownRequested;

        /// <inheritdoc />
        public void Dispose()
        {
            this.gpioConnection.Dispose();
            this.thread.Dispose();
        }

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        public void Shutdown()
        {
            var shutdownOutputPin = this.gpioConnectionDriver.Out(this.shutdownOutConnectorPin);
            shutdownOutputPin.Write(true);
            this.thread.Sleep(TimeSpan.FromMilliseconds(125));
            shutdownOutputPin.Write(false);
            this.thread.Sleep(TimeSpan.FromMilliseconds(200));
            shutdownOutputPin.Write(true);
            this.thread.Sleep(TimeSpan.FromMilliseconds(400));
            shutdownOutputPin.Write(false);
        }

        private void OnShutdown(bool state)
        {
            if (state)
            {
                this.gpioConnectionDriver.Out(this.shutdownInConnectorPin).Write(true);
                this.ShutdownRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}