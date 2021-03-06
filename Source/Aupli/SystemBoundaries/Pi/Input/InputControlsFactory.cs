﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputControlsFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Input
{
    using System;
    using System.Linq;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using Sundew.Pi.IO.Devices.RotaryEncoders;
    using Sundew.Pi.IO.Devices.RotaryEncoders.Ky040;

    /// <summary>
    /// Factory for creating the input controls.
    /// </summary>
    public static class InputControlsFactory
    {
        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>
        /// The input controls.
        /// </returns>
        public static InputControls Create(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            var lircConnection = new LircDevice();
            var playPauseButton = new PullDownButtonDevice(ConnectorPin.P1Pin15);
            var nextButton = new PullDownButtonDevice(ConnectorPin.P1Pin18);
            var previousButton = new PullDownButtonDevice(ConnectorPin.P1Pin16);
            var menuButton = new PullDownButtonDevice(ConnectorPin.P1Pin13);
            var rfidTransceiver = new Mfrc522Connection("/dev/spidev0.0", ConnectorPin.P1Pin22, gpioConnectionDriverFactory);
            var ky040Connection = new Ky040Device(
                ConnectorPin.P1Pin36,
                ConnectorPin.P1Pin38,
                ConnectorPin.P1Pin40,
                gpioConnectionDriverFactory,
                null);
            var buttonsGpioConnection = new GpioConnection(
                gpioConnectionDriverFactory,
                new[]
                {
                    playPauseButton.PinConfiguration,
                    nextButton.PinConfiguration,
                    previousButton.PinConfiguration,
                    menuButton.PinConfiguration,
                }.Where(x => x != null));

            return new InputControls(
                playPauseButton,
                nextButton,
                previousButton,
                menuButton,
                rfidTransceiver,
                lircConnection,
                ky040Connection,
                buttonsGpioConnection);
            /* new Rfid(),
             new Lirc(),
             new Rotary(),
             new Dispo());*/
        }

        private class Dispo : IDisposable
        {
            public void Dispose()
            {
            }
        }

        private class Lirc : ILircDevice
        {
            public event EventHandler<LircCommandEventArgs>? CommandReceived;

            public void Dispose()
            {
                if (false)
#pragma warning disable 162
                {
                    this.CommandReceived += (sender, args) => { };
                    this.CommandReceived?.Invoke(this, new LircCommandEventArgs(LircKeyCodes.Btn0, "L", 0, "Aupli"));
                }
#pragma warning restore 162
            }

            public void StartListening()
            {
            }

            public void Stop()
            {
            }
        }

        private class Rfid : IRfidConnection
        {
            public event EventHandler<TagDetectedEventArgs>? TagDetected;

            public void Dispose()
            {
                if (false)
#pragma warning disable 162
                {
                    this.TagDetected += (sender, args) => { };
                    this.TagDetected?.Invoke(this, new TagDetectedEventArgs(new Uid(1, 2, 3, 4)));
                }
#pragma warning restore 162
            }

            public void StartScanning()
            {
            }
        }

        private class Rotary : IRotaryEncoderWithButtonDevice
        {
            public event EventHandler<RotationEventArgs>? Rotated;

            public event EventHandler? Pressed;

            public void Dispose()
            {
                if (false)
#pragma warning disable 162
                {
                    this.Pressed += (sender, args) => { };
                    this.Rotated += (sender, args) => { };
                    this.Rotated?.Invoke(this, new RotationEventArgs(EncoderDirection.Clockwise));
                    this.Pressed?.Invoke(this, EventArgs.Empty);
                }
#pragma warning restore 162
            }

            public void Start()
            {
            }

            public void Stop()
            {
            }
        }
    }
}