// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserInterfaceBridge.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Interaction
{
    using Sundew.TextView.ApplicationFramework.Navigation;

    /// <summary>
    /// Bridge for getting the <see cref="IDisplay"/> and <see cref="ITextViewNavigator"/>.
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
        ITextViewNavigator TextViewNavigator { get; }
    }
}