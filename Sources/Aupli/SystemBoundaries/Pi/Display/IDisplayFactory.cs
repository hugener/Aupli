// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDisplayFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using System;
    using global::Pi.IO.GeneralPurpose;

    /// <summary>
    /// Interface for implementing a display factory.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDisplayFactory : IDisposable
    {
        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="hasBacklight">if set to <c>true</c> [has backlight].</param>
        /// <returns>
        /// A <see cref="Hd44780Display" />.
        /// </returns>
        IDisplay Create(IGpioConnectionDriverFactory gpioConnectionDriverFactory, bool hasBacklight);

        /// <summary>
        /// Disposes the specified text display device.
        /// </summary>
        /// <param name="display">The display.</param>
        void Dispose(IDisplay display);
    }
}