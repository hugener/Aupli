// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hd44780Display.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using global::Pi.IO.Devices.Displays.Hd44780;
    using Sundew.TextView.Pi.Drivers.Displays.Hd44780;

    /// <summary>
    /// Contains access to the Pi display.
    /// </summary>
    public class Hd44780Display : Hd44780TextDisplayDevice, IDisplay
    {
        private readonly Hd44780LcdDevice hd47780LcdDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hd44780Display"/> class.
        /// </summary>
        /// <param name="hd47780LcdDevice">The HD47780 connection.</param>
        /// <param name="hd47780LcdDeviceSettings">The HD47780 connection settings.</param>
        public Hd44780Display(Hd44780LcdDevice hd47780LcdDevice, Hd44780LcdDeviceSettings hd47780LcdDeviceSettings)
            : base(hd47780LcdDevice, hd47780LcdDeviceSettings)
        {
            this.hd47780LcdDevice = hd47780LcdDevice;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has backlight.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has backlight; otherwise, <c>false</c>.
        /// </value>
        public bool HasBacklight => this.hd47780LcdDevice.HasBacklight;

        /// <summary>
        /// Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[backlight enabled].
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [backlight enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool BacklightEnabled
        {
            get => this.hd47780LcdDevice.BacklightEnabled;
            set => this.hd47780LcdDevice.BacklightEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [backlight enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [backlight enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get
            {
                if (this.hd47780LcdDevice.HasBacklight)
                {
                    return this.hd47780LcdDevice.BacklightEnabled;
                }

                return this.hd47780LcdDevice.DisplayEnabled;
            }

            set
            {
                if (this.hd47780LcdDevice.HasBacklight)
                {
                    this.hd47780LcdDevice.BacklightEnabled = value;
                }
                else
                {
                    this.hd47780LcdDevice.DisplayEnabled = value;

                    if (!value)
                    {
                        this.hd47780LcdDevice.Clear();
                    }
                }
            }
        }
    }
}