// <copyright file="ITextAnimator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Sundew.Pi.ApplicationFramework.TextViewRendering.Animation
{
    /// <summary>
    /// Interface for implementing a text animator.
    /// </summary>
    public interface ITextAnimator
    {
        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A frame of the animated text.</returns>
        string GetFrame(string text, int width, int height);
    }
}