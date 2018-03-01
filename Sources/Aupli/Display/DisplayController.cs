// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Display
{
    using Pi.IO.Components.Displays.Hd44780;

    /// <summary>
    /// Controls the display back light.
    /// </summary>
    public class DisplayController
    {
        private readonly Hd44780LcdConnection hd44780LcdConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayController"/> class.
        /// </summary>
        /// <param name="hd44780LcdConnection">The HD44780 LCD connection.</param>
        public DisplayController(Hd44780LcdConnection hd44780LcdConnection)
        {
            this.hd44780LcdConnection = hd44780LcdConnection;
        }

        /// <summary>
        /// Sets the backlight.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public void SetBacklight(bool isEnabled)
        {
            this.hd44780LcdConnection.BacklightEnabled = isEnabled;
        }
    }
}