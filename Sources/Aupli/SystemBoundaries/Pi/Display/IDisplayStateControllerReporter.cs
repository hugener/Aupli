// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDisplayStateControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing a reporter for <see cref="IDisplayStateControllerReporter"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IDisplayStateControllerReporter : IReporter
    {
        /// <summary>
        /// Enableds the backlight.
        /// </summary>
        void EnabledDisplay();

        /// <summary>
        /// Disableds the backlight.
        /// </summary>
        void DisabledDisplay();
    }
}