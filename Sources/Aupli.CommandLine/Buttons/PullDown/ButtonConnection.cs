// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ButtonConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine.Buttons.PullDown
{
    using System;
    using Pi.IO.GeneralPurpose;

    public class ButtonConnection : IDisposable
    {
        private readonly GpioConnection gpioConnection;

        public ButtonConnection(ConnectorPin buttonConnectorPin)
        {
            var buttonPinConfiguration = buttonConnectorPin.Input().PullDown();
            buttonPinConfiguration.OnStatusChanged(this.OnButtonPressed);
            this.gpioConnection = new GpioConnection(buttonPinConfiguration);
        }

        public event EventHandler Pressed;

        public void Dispose()
        {
            this.gpioConnection.Close();
        }

        private void OnButtonPressed(bool state)
        {
            if (!state)
            {
                this.Pressed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}