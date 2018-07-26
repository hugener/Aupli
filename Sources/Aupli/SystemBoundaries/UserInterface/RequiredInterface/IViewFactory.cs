// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.RequiredInterface
{
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Factory for creating the different views.
    /// </summary>
    public interface IViewFactory
    {
        /// <summary>
        /// Gets the shutdown textView.
        /// </summary>
        /// <value>
        /// The shutdown textView.
        /// </value>
        ITextView ShutdownTextView { get; }

        /// <summary>
        /// Gets the menu textView.
        /// </summary>
        /// <value>
        /// The menu textView.
        /// </value>
        ITextView MenuTextView { get; }

        /// <summary>
        /// Gets the player textView.
        /// </summary>
        /// <value>
        /// The player textView.
        /// </value>
        ITextView PlayerTextView { get; }

        /// <summary>
        /// Gets the volume textView.
        /// </summary>
        /// <value>
        /// The volume textView.
        /// </value>
        ITextView VolumeTextView { get; }
    }
}