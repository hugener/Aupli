// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserInterfaceBridge.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Interaction
{
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Navigation;

    /// <summary>
    /// Bridge interface for display, text view navigation and input manager.
    /// </summary>
    public interface IUserInterfaceBridge
    {
        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        IDisplay Display { get; }

        /// <summary>
        /// Gets the text view navigator.
        /// </summary>
        /// <value>
        /// The text view navigator.
        /// </value>
        TextViewNavigator TextViewNavigator { get; }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input manager.
        /// </value>
        InputManager InputManager { get; }
    }
}