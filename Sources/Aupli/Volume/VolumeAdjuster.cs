// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeAdjuster.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using Aupli.Numeric;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Adjusts the volume within a certain range.
    /// </summary>
    public class VolumeAdjuster
    {
        private readonly Range<byte> volumeRange;
        private readonly double increment;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeAdjuster"/> class.
        /// </summary>
        /// <param name="volumeRange">The volume range.</param>
        /// <param name="volumePercentage">The volume percentage.</param>
        /// <param name="increment">The increment.</param>
        public VolumeAdjuster(Range<byte> volumeRange, Percentage volumePercentage, double increment)
        {
            this.volumeRange = volumeRange;
            this.increment = increment;
            this.Volume = this.FromPercentage(volumePercentage);
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public VolumeValue Volume { get; private set; }

        /// <summary>
        /// Adjusts the specified is increment.
        /// </summary>
        /// <param name="isIncrement">if set to <c>true</c> [is increment].</param>
        /// <returns>The new <see cref="VolumeValue"/>.</returns>
        public VolumeValue Adjust(bool isIncrement)
        {
            var change = isIncrement ? this.increment : -this.increment;
            var newVolume = this.Volume.Percentage + change;
            newVolume = newVolume.Limit(0, 1);
            return this.Volume = this.FromPercentage(newVolume);
        }

        /// <summary>
        /// Get the volume value from an absolute volume.
        /// </summary>
        /// <param name="absoluteVolume">The absolute volume.</param>
        /// <returns>The <see cref="VolumeValue"/>.</returns>
        public VolumeValue FromAbsolute(byte absoluteVolume)
        {
            return new VolumeValue(absoluteVolume, new Percentage(((double)absoluteVolume - this.volumeRange.Min) / (this.volumeRange.Max - this.volumeRange.Min)));
        }

        private VolumeValue FromPercentage(Percentage volumePercentage)
        {
            return new VolumeValue((byte)(((this.volumeRange.Max - this.volumeRange.Min) * volumePercentage) + this.volumeRange.Min), volumePercentage);
        }
    }
}
