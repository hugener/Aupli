// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Drivers.Displays.Hd44780
{
    using global::Pi.IO.Components.Displays.Hd44780;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Builder for custom charactors on a Hd44780 LCD display.
    /// </summary>
    public class CharacterContext : ICharacterContext
    {
        private readonly Hd44780LcdConnection hd44780LcdConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterContext"/> class.
        /// </summary>
        /// <param name="hd44780LcdConnection">The HD44780 LCD connection.</param>
        public CharacterContext(Hd44780LcdConnection hd44780LcdConnection)
        {
            this.hd44780LcdConnection = hd44780LcdConnection;
        }

        /// <summary>
        /// Sets the custom character.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="pattern">The pattern.</param>
        public void SetCustomCharacter(byte character, byte[] pattern)
        {
            this.hd44780LcdConnection.SetCustomCharacter(character, pattern);
        }
    }
}
