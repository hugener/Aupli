// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDisplayBacklightControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing a reporter for <see cref="IDisplayBacklightControllerReporter"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IDisplayBacklightControllerReporter : IReporter
    {
        /// <summary>
        /// Enableds the backlight.
        /// </summary>
        void EnabledBacklight();

        /// <summary>
        /// Disableds the backlight.
        /// </summary>
        void DisabledBacklight();
    }
}