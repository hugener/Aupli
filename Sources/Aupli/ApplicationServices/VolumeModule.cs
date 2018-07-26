﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices
{
    using Aupli.ApplicationServices.RequiredInterface.Amplifier;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Aupli.ApplicationServices.RequiredInterface.Volume;
    using Aupli.ApplicationServices.Volume;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Represents the domain logic module for volume.
    /// </summary>
    public class VolumeModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeModule" /> class.
        /// </summary>
        /// <param name="volumeRepository">The volume repository.</param>
        /// <param name="volumeIncrementStep">The volume increment step.</param>
        /// <param name="playerStatusUpdater">The player status updater.</param>
        /// <param name="volumeStatusUpdater">The volume status updater.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="volumeServiceReporter">The volume service reporter.</param>
        public VolumeModule(
            IVolumeRepository volumeRepository,
            Percentage volumeIncrementStep,
            IPlayerStatusUpdater playerStatusUpdater,
            IVolumeStatusUpdater volumeStatusUpdater,
            IAmplifier amplifier,
            IVolumeServiceReporter volumeServiceReporter)
        {
            // Create application services
            this.VolumeService = new VolumeService(
                volumeRepository,
                new VolumeAdjustmentService(volumeRepository.Volume, volumeIncrementStep),
                playerStatusUpdater,
                volumeStatusUpdater,
                amplifier,
                volumeServiceReporter);
        }

        /// <summary>
        /// Gets the volume service.
        /// </summary>
        /// <value>
        /// The volume service.
        /// </value>
        public VolumeService VolumeService { get; }
    }
}