// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewTimer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    using System;

    /// <summary>
    /// A timer to be used in implemtations of <see cref="ITextView"/> for animation.
    /// </summary>
    public interface IViewTimer
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        event EventHandler Tick;

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <param name="startDelay">The delay before the first occurence.</param>
        void Start(TimeSpan startDelay);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Restarts the specified start delay.
        /// </summary>
        /// <param name="startDelay">The start delay.</param>
        void Restart(TimeSpan startDelay);
    }
}