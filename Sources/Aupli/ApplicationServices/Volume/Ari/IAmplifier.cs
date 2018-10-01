// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAmplifier.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Ari
{
    using Sundew.Base.Numeric;

    /// <summary>
    /// Interface for implementing an amplifier.
    /// </summary>
    public interface IAmplifier
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        bool IsMuted { get; set; }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="volume">The volume value.</param>
        void SetVolume(Percentage volume);
    }
}