// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Amplifier.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine.Amplifiers.Max9744
{
    using System;
    using Pi.IO.GeneralPurpose;
    using Pi.IO.InterIntegratedCircuit;

    public class Max9744Connection : IDisposable
    {
        private readonly I2cDeviceConnection connection;
        private readonly I2cDriver i2CDriver;
        private readonly OutputPinConfiguration shutdownPin;
        private readonly GpioConnection gpioConnection;
        private readonly OutputPinConfiguration mutePin;

        public Max9744Connection(ConnectorPin mutePin, ConnectorPin shutdownPin, ProcessorPin sdaPin, ProcessorPin sclPin)
        {
            // connect volume via i2c
            this.i2CDriver = new I2cDriver(sdaPin, sclPin);
            this.connection = this.i2CDriver.Connect(0x4b);

            // connect shutdown via gpio
            this.mutePin = mutePin.Output().Revert();
            this.shutdownPin = shutdownPin.Output().Revert();
            this.gpioConnection = new GpioConnection(this.shutdownPin, this.mutePin);
        }

        public void SetVolume(byte value)
        {
            var data = (byte)(value / 255f * 63);
            Console.WriteLine("Set volume: " + value);
            this.connection.WriteByte(data);
        }

        public void ToggleMute()
        {
            if (this.gpioConnection[this.mutePin])
            {
                Console.WriteLine("Mute off");
                this.gpioConnection[this.mutePin] = false;
            }
            else
            {
                Console.WriteLine("Mute on");
                this.gpioConnection[this.mutePin] = true;
            }
        }

        public void SetShutdownState(bool enabled)
        {
            if (enabled)
            {
                Console.WriteLine("Shutdown on");
                this.gpioConnection[this.shutdownPin] = true;
            }
            else
            {
                Console.WriteLine("Shutdown off");
                this.gpioConnection[this.shutdownPin] = false;
            }
        }

        public void Dispose()
        {
            this.i2CDriver?.Dispose();
            this.gpioConnection?.Close();
        }

        public byte GetVolume()
        {
            var value = this.connection.ReadByte();
            var volume = (byte)(value * 255f / 63);
            Console.WriteLine("Get volume: " + volume);
            return volume;
        }
    }
}