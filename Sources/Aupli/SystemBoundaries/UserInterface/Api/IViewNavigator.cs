// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Api
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for navigating between views.
    /// </summary>
    public interface IViewNavigator
    {
        /// <summary>
        /// Navigates to player view asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        Task NavigateToPlayerViewAsync();
    }
}