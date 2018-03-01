// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mfrc522Connection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.RfidTransceivers.Mfrc522
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Pi;
    using global::Pi.IO.GeneralPurpose;
    using global::Pi.System.Threading;

    /// <summary>
    /// A connection to a <see cref="Mfrc522Device"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Mfrc522Connection : IDisposable
    {
        private readonly string spiDevicePath;
        private readonly ConnectorPin? resetConnectorPin;
        private readonly IThread thread;
        private readonly Mfrc522Device mfrc522Device;
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private CancellationTokenSource cancellationTokenSource;
        private Task scanningTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mfrc522Connection" /> class.
        /// </summary>
        /// <param name="spiDevicePath">The spi device path.</param>
        /// <param name="resetConnectorPin">The reset connector pin.</param>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="threadFactory">The thread factory.</param>
        public Mfrc522Connection(
            string spiDevicePath,
            ConnectorPin? resetConnectorPin,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory = null,
            IThreadFactory threadFactory = null)
        {
            this.spiDevicePath = spiDevicePath;
            this.resetConnectorPin = resetConnectorPin;
            this.thread = ThreadFactory.EnsureThreadFactory(threadFactory).Create();
            this.gpioConnectionDriver = GpioConnectionDriverFactory
                .EnsureGpioConnectionDriverFactory(gpioConnectionDriverFactory).Create();
            this.mfrc522Device = new Mfrc522Device(this.thread, this.gpioConnectionDriver);
        }

        /// <summary>
        /// Occurs when a tag is detected.
        /// </summary>
        public event EventHandler<TagDetectedEventArgs> TagDetected;

        /// <summary>
        /// Starts scanning for tags.
        /// </summary>
        public void StartScanning()
        {
            if (this.scanningTask != null)
            {
                return;
            }

            this.mfrc522Device.Initialize(this.spiDevicePath, this.resetConnectorPin);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.scanningTask = new TaskFactory().StartNew(
                this.CheckForTags,
                this.cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        void IDisposable.Dispose()
        {
            this.cancellationTokenSource?.Cancel();
            this.scanningTask?.Wait();
            this.cancellationTokenSource?.Dispose();
            this.mfrc522Device.Dispose();
            this.gpioConnectionDriver.Dispose();
            this.scanningTask = null;
            this.cancellationTokenSource = null;
            this.thread.Dispose();
        }

        private void CheckForTags()
        {
            try
            {
                var cancellationToken = this.cancellationTokenSource.Token;

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (this.mfrc522Device.IsTagPresent())
                    {
                        var uid = this.mfrc522Device.ReadUid();
                        this.mfrc522Device.HaltTag();
                        this.TagDetected?.Invoke(this, new TagDetectedEventArgs(uid));
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    this.thread.Sleep(TimeSpan.FromMilliseconds(50));
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}