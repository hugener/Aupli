// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using System;
    using Aupli.Numeric;

    /// <summary>
    /// Contains info about the volume changed event.
    /// </summary>
    public class VolumeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeEventArgs"/> class.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        public VolumeEventArgs(Percentage volume, bool isMuted)
        {
            this.Volume = volume;
            this.IsMuted = isMuted;
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted { get; }
    }
}