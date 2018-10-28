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
    using Sundew.Base.Computation;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Contains access to the Pi display.
    /// </summary>
    public class Hd44780Display : IDisplay
    {
        private readonly Hd44780LcdDevice hd47780LcdDevice;
        private readonly Hd44780LcdDeviceSettings hd47780LcdDeviceSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hd44780Display"/> class.
        /// </summary>
        /// <param name="hd47780LcdDevice">The HD47780 connection.</param>
        /// <param name="hd47780LcdDeviceSettings">The HD47780 connection settings.</param>
        public Hd44780Display(Hd44780LcdDevice hd47780LcdDevice, Hd44780LcdDeviceSettings hd47780LcdDeviceSettings)
        {
            this.hd47780LcdDevice = hd47780LcdDevice;
            this.hd47780LcdDeviceSettings = hd47780LcdDeviceSettings;
            this.Size = new Size(this.hd47780LcdDeviceSettings.ScreenWidth, this.hd47780LcdDeviceSettings.ScreenHeight);
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

        /// <summary>
        /// Gets or sets a value indicating whether [cursor enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorEnabled
        {
            get => this.hd47780LcdDevice.CursorEnabled;
            set => this.hd47780LcdDevice.CursorEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor blinking].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorBlinking
        {
            get => this.hd47780LcdDevice.CursorBlinking;
            set => this.hd47780LcdDevice.CursorBlinking = value;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Size Size { get; }

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public Point CursorPosition => new Point(this.hd47780LcdDevice.CursorPosition.Column, this.hd47780LcdDevice.CursorPosition.Row);

        /// <summary>
        /// Tries the create character context.
        /// </summary>
        /// <returns>
        /// The result with an <see cref="T:Sundew.Pi.ApplicationFramework.TextViewRendering.ICharacterContext" /> if successfull.
        /// </returns>
        public Result<ICharacterContext> TryCreateCharacterContext()
        {
            return Result.Success<ICharacterContext>(new CharacterContext(
                this.hd47780LcdDevice,
                new Size(this.hd47780LcdDeviceSettings.PatternWidth, this.hd47780LcdDeviceSettings.PatternHeight)));
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            this.hd47780LcdDevice.WriteLine(text);
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            this.hd47780LcdDevice.Write(text);
        }

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteFormat(string format, params object[] values)
        {
            this.hd47780LcdDevice.Write(format, values);
        }

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteLineFormat(string format, params object[] values)
        {
            this.hd47780LcdDevice.WriteLine(format, values);
        }

        /// <summary>
        /// Homes this instance.
        /// </summary>
        public void Home()
        {
            this.hd47780LcdDevice.Home();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.hd47780LcdDevice.Clear();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            this.hd47780LcdDevice.SetCursorPosition(new Hd44780Position { Column = x, Row = y });
        }

        /// <summary>
        /// Moves the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void Move(int offset)
        {
            this.hd47780LcdDevice.Move(offset);
        }
    }
}