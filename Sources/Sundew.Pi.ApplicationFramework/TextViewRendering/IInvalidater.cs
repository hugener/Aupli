// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInvalidater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    using global::Pi.Timers;

    /// <summary>
    /// Interface for <see cref="ITextView"/> to indicate whether they have to be invalidated.
    /// </summary>
    public interface IInvalidater
    {
        /// <summary>
        /// Gets the timer.
        /// </summary>
        /// <value>
        /// The timer.
        /// </value>
        ITimer Timer { get; }

        /// <summary>
        /// Indicates that the <see cref="ITextView"/> needs to be rendered.
        /// </summary>
        void Invalidate();
    }
}