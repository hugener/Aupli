// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyInputArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Input
{
    using System;

    /// <summary>
    /// Event arguments for <see cref="KeyInput"/>.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class KeyInputArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyInputArgs"/> class.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        public KeyInputArgs(KeyInput keyInput)
        {
            this.KeyInput = keyInput;
        }

        /// <summary>
        /// Gets the key input.
        /// </summary>
        /// <value>
        /// The key input.
        /// </value>
        public KeyInput KeyInput { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Key: " + this.KeyInput;
        }
    }
}