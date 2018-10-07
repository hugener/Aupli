// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDisplay.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Interaction
{
    using Sundew.Pi.ApplicationFramework.DeviceInterface;

    /// <summary>
    /// Interface for implementing a display.
    /// </summary>
    /// <seealso cref="Sundew.Pi.ApplicationFramework.DeviceInterface.ITextDisplayDevice" />
    public interface IDisplay : ITextDisplayDevice
    {
        /// <summary>
        /// Gets or sets a value indicating whether [backlight enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [backlight enabled]; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }
    }
}