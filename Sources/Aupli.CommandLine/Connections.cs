// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Connections.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine
{
    using System;
    using Aupli.CommandLine.Amplifiers.Max9744;
    using Aupli.CommandLine.Buttons.PullDown;
    using Aupli.CommandLine.Encoders.Ky040;
    using Aupli.CommandLine.Rfid.Mfrc522;
    using Pi.IO;
    using Pi.IO.Components.Displays.Hd44780;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;

    public class Connections : IDisposable
    {
        private readonly Disposer disposer;

        public Connections()
        {
            var gpioConnectionDriver = GpioConnectionSettings.DefaultDriver;
            this.MenuButton = new ButtonConnection(ConnectorPin.P1Pin13);
            this.PlayPauseButton = new ButtonConnection(ConnectorPin.P1Pin15);
            this.NextButton = new ButtonConnection(ConnectorPin.P1Pin18);
            this.PreviousButton = new ButtonConnection(ConnectorPin.P1Pin16);
            this.VolumeInput = new Ky040Connection(ConnectorPin.P1Pin36, ConnectorPin.P1Pin38, ConnectorPin.P1Pin40);
            this.Amplifier = new Max9744Connection(
                ConnectorPin.P1Pin07,
                ConnectorPin.P1Pin11,
                ProcessorPin.Pin02,
                ProcessorPin.Pin03);
            this.Amplifier.SetShutdownState(false);
            this.Amplifier.SetVolume(0);
            var dataPins = new IOutputBinaryPin[]
            {
                gpioConnectionDriver.Out(ConnectorPin.P1Pin31),
                gpioConnectionDriver.Out(ConnectorPin.P1Pin33),
                gpioConnectionDriver.Out(ConnectorPin.P1Pin35),
                gpioConnectionDriver.Out(ConnectorPin.P1Pin37)
            };
            this.Display = new Hd44780LcdConnection(
                new Hd44780LcdConnectionSettings
                {
                    ScreenHeight = 2,
                    ScreenWidth = 16
                },
                new Hd44780Pins(gpioConnectionDriver.Out(ConnectorPin.P1Pin29), gpioConnectionDriver.Out(ConnectorPin.P1Pin32), dataPins));
            this.Display.Clear();
            this.RfidController = new Mfrc522Connection("/dev/spidev0.0", ConnectorPin.P1Pin22);
            this.disposer = new Disposer(
                this.MenuButton,
                this.PlayPauseButton,
                this.NextButton,
                this.PreviousButton,
                this.VolumeInput,
                this.RfidController,
                this.Amplifier,
                this.Display);
        }

        public Hd44780LcdConnection Display { get; }

        public Max9744Connection Amplifier { get; }

        public Mfrc522Connection RfidController { get; }

        public Ky040Connection VolumeInput { get; }

        public ButtonConnection PreviousButton { get; }

        public ButtonConnection NextButton { get; }

        public ButtonConnection PlayPauseButton { get; }

        public ButtonConnection MenuButton { get; }

        public void Dispose()
        {
            this.disposer.Dispose();
        }
    }
}