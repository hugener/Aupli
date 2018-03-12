// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Display
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing navigation.
    /// </summary>
    public interface IViewNavigator
    {
        /// <summary>
        /// Navigates to startup textView.
        /// </summary>
        void NavigateToStartupView();

        /// <summary>
        /// Navigates to player textView.
        /// </summary>
        /// <returns>An async task.</returns>
        Task NavigateToPlayerViewAsync();

        /// <summary>
        /// Navigates to volume textView.
        /// </summary>
        /// <param name="activeTimeSpan">The active time span.</param>
        /// <returns>An async task.</returns>
        Task NavigateToVolumeViewAsync(TimeSpan activeTimeSpan);

        /// <summary>
        /// Navigates to shutdown textView.
        /// </summary>
        void NavigateToShutdownView();

        /// <summary>
        /// Navigates to menu textView.
        /// </summary>
        void NavigateToMenuView();

        /// <summary>
        /// Navigates back.
        /// </summary>
        void GoBack();
    }
}