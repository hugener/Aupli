// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputControls.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Interaction
{
    using System;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;

    /// <summary>
    /// Contains the input controls.
    /// </summary>
    public class InputControls
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputControls" /> class.
        /// </summary>
        /// <param name="playPauseButton">The play pause button.</param>
        /// <param name="nextButton">The next button.</param>
        /// <param name="previousButton">The previous button.</param>
        /// <param name="menuButton">The menu button.</param>
        /// <param name="rfidTransceiver">The rfid transceiver.</param>
        /// <param name="remoteControl">The lirc connection.</param>
        /// <param name="rotaryEncoder">The ky040 connection.</param>
        /// <param name="buttonsGpioConnection">The buttons gpio connection.</param>
        public InputControls(
            IButtonDevice playPauseButton,
            IButtonDevice nextButton,
            IButtonDevice previousButton,
            IButtonDevice menuButton,
            IRfidConnection rfidTransceiver,
            ILircDevice remoteControl,
            IRotaryEncoderWithButtonDevice rotaryEncoder,
            IDisposable buttonsGpioConnection)
        {
            this.PlayPauseButton = playPauseButton;
            this.NextButton = nextButton;
            this.PreviousButton = previousButton;
            this.MenuButton = menuButton;
            this.RfidTransceiver = rfidTransceiver;
            this.RemoteControl = remoteControl;
            this.RotaryEncoder = rotaryEncoder;
            this.ButtonsGpioConnection = buttonsGpioConnection;
        }

        /// <summary>
        /// Gets the play pause button.
        /// </summary>
        /// <value>
        /// The play pause button.
        /// </value>
        public IButtonDevice PlayPauseButton { get; }

        /// <summary>
        /// Gets the next button.
        /// </summary>
        /// <value>
        /// The next button.
        /// </value>
        public IButtonDevice NextButton { get; }

        /// <summary>
        /// Gets the previous button.
        /// </summary>
        /// <value>
        /// The previous button.
        /// </value>
        public IButtonDevice PreviousButton { get; }

        /// <summary>
        /// Gets the menu button.
        /// </summary>
        /// <value>
        /// The menu button.
        /// </value>
        public IButtonDevice MenuButton { get; }

        /// <summary>
        /// Gets the rfid transceiver.
        /// </summary>
        /// <value>
        /// The rfid transceiver.
        /// </value>
        public IRfidConnection RfidTransceiver { get; }

        /// <summary>
        /// Gets the lirc connection.
        /// </summary>
        /// <value>
        /// The lirc connection.
        /// </value>
        public ILircDevice RemoteControl { get; }

        /// <summary>
        /// Gets the ky040 connection.
        /// </summary>
        /// <value>
        /// The ky040 connection.
        /// </value>
        public IRotaryEncoderWithButtonDevice RotaryEncoder { get; }

        /// <summary>
        /// Gets the buttons.
        /// </summary>
        /// <value>
        /// The buttons.
        /// </value>
        public IDisposable ButtonsGpioConnection { get; }
    }
}