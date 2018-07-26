// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputControlsFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Interaction
{
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using Sundew.Pi.IO.Devices.RotaryEncoders.Ky040;

    /// <summary>
    /// Factory for creating the input controls.
    /// </summary>
    public static class InputControlsFactory
    {
        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <returns>
        /// The input controls.
        /// </returns>
        public static InputControls Create(IGpioConnectionDriver gpioConnectionDriver)
        {
            var lircConnection = new LircDevice();
            var playPauseButton = new PullDownButtonDevice(ConnectorPin.P1Pin15);
            var nextButton = new PullDownButtonDevice(ConnectorPin.P1Pin18);
            var previousButton = new PullDownButtonDevice(ConnectorPin.P1Pin16);
            var menuButton = new PullDownButtonDevice(ConnectorPin.P1Pin13);
            var rfidTransceiver = new Mfrc522Connection("/dev/spidev0.0", ConnectorPin.P1Pin22);
            var ky040Connection = new Ky040Device(
                gpioConnectionDriver,
                ConnectorPin.P1Pin36,
                ConnectorPin.P1Pin38,
                ConnectorPin.P1Pin40,
                null);

            return new InputControls(
                playPauseButton,
                nextButton,
                previousButton,
                menuButton,
                rfidTransceiver,
                lircConnection,
                ky040Connection);
        }
    }
}