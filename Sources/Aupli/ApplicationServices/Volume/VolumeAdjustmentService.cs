// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeAdjustmentService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Interface.Volume;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Manages adjusting the volume.
    /// </summary>
    public class VolumeAdjustmentService : IVolumeAdjustmentService
    {
        private readonly Percentage incrementStep;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeAdjustmentService" /> class.
        /// </summary>
        /// <param name="initialVolume">The initial volume.</param>
        /// <param name="incrementStep">The increment.</param>
        public VolumeAdjustmentService(Percentage initialVolume, Percentage incrementStep)
        {
            this.incrementStep = incrementStep;
            this.Volume = this.FromPercentage(initialVolume);
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Shared.Volume.Volume Volume { get; private set; }

        /// <summary>
        /// Adjusts the specified is increment.
        /// </summary>
        /// <param name="isIncrement">if set to <c>true</c> [is increment].</param>
        /// <returns>The new <see cref="Volume"/>.</returns>
        public Task<Shared.Volume.Volume> AdjustVolumeAsync(bool isIncrement)
        {
            var change = isIncrement ? this.incrementStep : -this.incrementStep;
            var newVolume = this.Volume.Percentage + change;
            newVolume = newVolume.Limit(0, 1);
            this.Volume = this.FromPercentage(newVolume);
            return Task.FromResult(this.Volume);
        }

        private Shared.Volume.Volume FromPercentage(Percentage volumePercentage)
        {
            return new Shared.Volume.Volume(volumePercentage);
        }
    }
}