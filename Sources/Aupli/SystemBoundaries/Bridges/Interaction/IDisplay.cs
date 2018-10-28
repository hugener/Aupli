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

        /// <summary>
        /// Gets a value indicating whether this instance has backlight.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has backlight; otherwise, <c>false</c>.
        /// </value>
        bool HasBacklight { get; }

        /// <summary>
        /// Gets or sets a value indicating whether {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}[backlight enabled].
        /// </summary>
        /// <value>
        /// {D255958A-8513-4226-94B9-080D98F904A1}  <c>true</c> if [backlight enabled]; otherwise, <c>false</c>.
        /// </value>
        bool BacklightEnabled { get; set; }
    }
}