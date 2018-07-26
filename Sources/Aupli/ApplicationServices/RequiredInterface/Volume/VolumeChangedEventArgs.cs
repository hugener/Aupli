// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeChangedEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.RequiredInterface.Volume
{
    using System;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Event args for when volume is changed.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class VolumeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="volume">The volume.</param>
        public VolumeChangedEventArgs(Percentage volume)
        {
            this.Volume = volume;
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume { get; }
    }
}