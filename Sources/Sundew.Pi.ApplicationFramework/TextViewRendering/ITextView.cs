// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    /// <summary>
    /// Interface for implementing text views.
    /// </summary>
    public interface ITextView
    {
        /// <summary>
        /// Called when is showing.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        void OnShowing(IInvalidater invalidater, ICharacterContext characterContext);

        /// <summary>
        /// Called when the text view should render.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        void Render(IRenderContext renderContext);

        /// <summary>
        /// Called when the view is closing.
        /// </summary>
        void OnClosing();
    }
}