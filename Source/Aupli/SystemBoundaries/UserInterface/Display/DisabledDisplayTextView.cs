// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisabledDisplayTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Display
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// An empty text view for when the display is off.
    /// </summary>
    /// <seealso cref="Sundew.TextView.ApplicationFramework.TextViewRendering.ITextView" />
    public class DisabledDisplayTextView : ITextView
    {
        /// <summary>
        /// Gets the input targets.
        /// </summary>
        /// <value>
        /// The input targets.
        /// </value>
        public IEnumerable<object>? InputTargets => null;

        /// <summary>
        /// Called when is showing.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        /// <returns>
        /// An async task.
        /// </returns>
        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext? characterContext)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when the text view should render.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        public void OnDraw(IRenderContext renderContext)
        {
            renderContext.Clear();
        }

        /// <summary>
        /// Called when the view is closing.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }
    }
}