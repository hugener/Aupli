// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeAdjuster.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume
{
    using System;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Manages adjusting the volume.
    /// </summary>
    public class VolumeAdjuster
    {
        private readonly Percentage incrementStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeAdjuster" /> class.
        /// </summary>
        /// <param name="incrementStep">The increment.</param>
        public VolumeAdjuster(Percentage incrementStep)
        {
            this.incrementStep = incrementStep;
        }

        /// <summary>
        /// Adjusts the specified is increment.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <param name="isIncrement">if set to <c>true</c> [is increment].</param>
        /// <returns>
        /// The new volume.
        /// </returns>
        public Percentage AdjustVolume(Percentage volume, bool isIncrement)
        {
            var change = isIncrement ? this.incrementStep : -this.incrementStep;
            var newVolume = volume + change;
            newVolume = newVolume.Limit(0, 1).Round(2);
            return newVolume;
        }
    }
}