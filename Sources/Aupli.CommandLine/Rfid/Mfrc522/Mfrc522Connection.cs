// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mfrc522Connection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine.Rfid.Mfrc522
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Pi.IO.GeneralPurpose;

    public class Mfrc522Connection : IDisposable
    {
        private readonly string spiDevicePath;
        private readonly ConnectorPin resetConnectorPin;
        private readonly Mfrc522Device mfrc522Device;
        private CancellationTokenSource cancellationTokenSource;
        private Task task;

        public Mfrc522Connection(string spiDevicePath, ConnectorPin resetConnectorPin)
        {
            this.spiDevicePath = spiDevicePath;
            this.resetConnectorPin = resetConnectorPin;
            this.mfrc522Device = new Mfrc522Device();
        }

        public event EventHandler<TagDetectedEventArgs> TagDetected;

        public void StartScanning()
        {
            if (this.task != null)
            {
                return;
            }

            this.mfrc522Device.Initialize(this.spiDevicePath, this.resetConnectorPin);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.task = Task.Run((Action)this.CheckForTags, this.cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
            this.task.Wait();
            this.mfrc522Device.Dispose();
        }

        private void CheckForTags()
        {
            while (!this.cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (this.mfrc522Device.IsTagPresent())
                {
                    var uid = this.mfrc522Device.ReadUid();
                    this.mfrc522Device.HaltTag();
                    this.TagDetected?.Invoke(this, new TagDetectedEventArgs(uid));
                }

                if (this.cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                Pi.Timers.Timer.Sleep(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}