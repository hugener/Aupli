// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingContextFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Drivers.Displays.Hd44780
{
    using ApplicationFramework.TextViewRendering;
    using global::Pi.IO.Components.Displays.Hd44780;

    /// <summary>
    /// Rendering context factory for an HD44780 display.
    /// </summary>
    /// <seealso cref="IRenderingContextFactory" />
    public class RenderingContextFactory : IRenderingContextFactory
    {
        private readonly Hd44780LcdConnection hd44780LcdConnection;
        private readonly Hd44780LcdConnectionSettings hd44780LcdConnectionSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingContextFactory"/> class.
        /// </summary>
        /// <param name="hd44780LcdConnection">The HD44780 LCD connection.</param>
        /// <param name="hd44780LcdConnectionSettings">The HD44780 LCD connection settings.</param>
        public RenderingContextFactory(Hd44780LcdConnection hd44780LcdConnection, Hd44780LcdConnectionSettings hd44780LcdConnectionSettings)
        {
            this.hd44780LcdConnection = hd44780LcdConnection;
            this.hd44780LcdConnectionSettings = hd44780LcdConnectionSettings;
        }

        /// <summary>
        /// Creates the render context.
        /// </summary>
        /// <returns>
        /// A <see cref="RenderingContext" />.
        /// </returns>
        public IRenderingContext CreateRenderingContext()
        {
            return new RenderingContext(
                this.hd44780LcdConnection,
                this.hd44780LcdConnectionSettings.ScreenWidth,
                this.hd44780LcdConnectionSettings.ScreenHeight);
        }

        /// <summary>
        /// Tries the create custom character builder.
        /// </summary>
        /// <param name="characterContext">The character context.</param>
        /// <returns>
        /// <c>true</c>, if custom characters are supported, otherwise <c>false</c>.
        /// </returns>
        public bool TryCreateCustomCharacterBuilder(out ICharacterContext characterContext)
        {
            characterContext = new CharacterContext(this.hd44780LcdConnection);
            return true;
        }
    }
}