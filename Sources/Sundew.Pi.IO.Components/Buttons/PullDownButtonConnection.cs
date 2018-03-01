// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PullDownButtonConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.Buttons
{
    using System;
    using global::Pi.IO.GeneralPurpose;

    /// <summary>
    /// Represents a connection to a button.
    /// </summary>
    public class PullDownButtonConnection : IGpioConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullDownButtonConnection"/> class.
        /// </summary>
        /// <param name="buttonConnectorPin">The button connector pin.</param>
        public PullDownButtonConnection(ConnectorPin buttonConnectorPin)
        {
            this.PinConfiguration = buttonConnectorPin.Input().PullDown();
            this.PinConfiguration.OnStatusChanged(this.OnButtonPressed);
        }

        /// <summary>
        /// Occurs when the button is pressed.
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Gets the button pin configuration.
        /// </summary>
        /// <value>
        /// The button pin configuration.
        /// </value>
        public PinConfiguration PinConfiguration { get; }

        private void OnButtonPressed(bool state)
        {
            if (state)
            {
                this.Pressed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}