// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewNavigatorReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Ari
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reports from the <see cref="Aupli.SystemBoundaries.UserInterface.Internal.ViewNavigator"/>.
    /// </summary>
    public interface IViewNavigatorReporter : IReporter
    {
        /// <summary>
        /// Navigates to player text view.
        /// </summary>
        void NavigateToPlayerTextView();

        /// <summary>
        /// Navigates to volume text view.
        /// </summary>
        void NavigateToVolumeTextView();

        /// <summary>
        /// Navigates to menu text view.
        /// </summary>
        void NavigateToMenuTextView();

        /// <summary>
        /// Navigates to shutdown text view.
        /// </summary>
        void NavigateToShutdownTextView();

        /// <summary>
        /// Navigates back.
        /// </summary>
        void NavigateBack();
    }
}