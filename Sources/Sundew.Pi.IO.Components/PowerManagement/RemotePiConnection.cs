// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemotePiConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.PowerManagement
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Pi.IO.GeneralPurpose;
    using global::Pi.System.Threading;
    using Sundew.Base.Time;

    /// <summary>
    /// Represents a connection to the RemotePI for shutting down PI.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RemotePiConnection : IDisposable
    {
        internal static readonly TimeSpan PowerOffTimeSpan = TimeSpan.FromMinutes(4);
        internal static readonly TimeSpan ShutdownTimeSpan = TimeSpan.FromSeconds(10);
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly ConnectorPin shutdownInConnectorPin;
        private readonly ConnectorPin shutdownOutConnectorPin;
        private readonly IOperationSystemShutdown operationSystemShutdown;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IThread thread;
        private readonly GpioConnection gpioConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePiConnection" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="shutdownInConnectorPin">The shutdown in connector pin.</param>
        /// <param name="shutdownOutConnectorPin">The shutdown out connector pin.</param>
        /// <param name="operationSystemShutdown">The operation system shutdown.</param>
        public RemotePiConnection(
            IGpioConnectionDriver gpioConnectionDriver,
            ConnectorPin shutdownInConnectorPin,
            ConnectorPin shutdownOutConnectorPin,
            IOperationSystemShutdown operationSystemShutdown)
            : this(
                  gpioConnectionDriver,
                  shutdownInConnectorPin,
                  shutdownOutConnectorPin,
                  operationSystemShutdown,
                  null,
                  null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePiConnection" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="shutdownInConnectorPin">The shutdown in connector pin.</param>
        /// <param name="shutdownOutConnectorPin">The shutdown out connector pin.</param>
        /// <param name="operationSystemShutdown">The operation system shutdown.</param>
        /// <param name="threadFactory">The thread factory.</param>
        /// <param name="dateTimeProvider">The date time provider.</param>
        public RemotePiConnection(
                    IGpioConnectionDriver gpioConnectionDriver,
                    ConnectorPin shutdownInConnectorPin,
                    ConnectorPin shutdownOutConnectorPin,
                    IOperationSystemShutdown operationSystemShutdown,
                    IThreadFactory threadFactory,
                    IDateTimeProvider dateTimeProvider)
        {
            this.gpioConnectionDriver = gpioConnectionDriver;
            this.shutdownInConnectorPin = shutdownInConnectorPin;
            this.shutdownOutConnectorPin = shutdownOutConnectorPin;
            this.operationSystemShutdown = operationSystemShutdown;
            this.dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            this.thread = ThreadFactory.EnsureThreadFactory(threadFactory).Create();
            var pinConfiguration = shutdownInConnectorPin.Input().PullDown();
            pinConfiguration.OnStatusChanged(this.OnShutdown);
            this.gpioConnection = new GpioConnection(pinConfiguration);
        }

        /// <summary>
        /// Occurs when remote pi requests system shutdown.
        /// </summary>
        public event EventHandler<ShutdownEventArgs> ShuttingDown;

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
                var cancellationTokenSource = new CancellationTokenSource();
                var shutdownEventArgs = new ShutdownEventArgs(cancellationTokenSource, this.dateTimeProvider.Now);
                Task.Run(() => this.ShutdownAsync(cancellationTokenSource.Token), cancellationTokenSource.Token)
                    .ContinueWith((task, _) => cancellationTokenSource.Dispose(), null);
                this.ShuttingDown?.Invoke(this, shutdownEventArgs);
            }
        }

        private async Task ShutdownAsync(CancellationToken token)
        {
            try
            {
                this.gpioConnectionDriver.Out(this.shutdownInConnectorPin).Write(true);
                await Task.Delay(ShutdownTimeSpan, token);
                this.operationSystemShutdown.Shutdown();
            }
            catch (OperationCanceledException)
            {
                this.gpioConnectionDriver.Out(this.shutdownInConnectorPin).Write(false);
            }
        }
    }
}