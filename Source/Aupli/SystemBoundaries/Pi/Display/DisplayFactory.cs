﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Pi.Display.Api;
    using global::Pi.IO.Devices.Displays.Hd44780;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;

    /// <summary>
    /// Creates an Hd47780 display device.
    /// </summary>
    public class DisplayFactory : IDisplayFactory
    {
        private readonly DisposingDictionary<IDisplay> textDisplayDevices = new DisposingDictionary<IDisplay>();

        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="hasBacklight">if set to <c>true</c> [has backlight].</param>
        /// <returns>
        /// A <see cref="Hd44780Display" />.
        /// </returns>
        public IDisplay Create(IGpioConnectionDriverFactory gpioConnectionDriverFactory, bool hasBacklight)
        {
            var hd44780LcdDeviceSettings = new Hd44780LcdDeviceSettings
            {
                ScreenHeight = 2,
                ScreenWidth = 16,
                Encoding = new Hd44780A00Encoding(),
            };
            var backlight = hasBacklight ? new ConnectorPin?(ConnectorPin.P1Pin26) : null;
            var hd44780LcdDevice = new Hd44780LcdDevice(
                hd44780LcdDeviceSettings,
                gpioConnectionDriverFactory,
                ConnectorPin.P1Pin29,
                ConnectorPin.P1Pin32,
                new Hd44780DataPins(
                    ConnectorPin.P1Pin31,
                    ConnectorPin.P1Pin33,
                    ConnectorPin.P1Pin35,
                    ConnectorPin.P1Pin37),
                backlight);
            return this.textDisplayDevices.Add(new Hd44780Display(hd44780LcdDevice, hd44780LcdDeviceSettings), hd44780LcdDevice);
        }

        /// <summary>
        /// Disposes the specified text display device.
        /// </summary>
        /// <param name="display">The display.</param>
        public void Dispose(IDisplay display)
        {
            this.textDisplayDevices.Dispose(display);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.textDisplayDevices.Dispose();
        }
    }
}