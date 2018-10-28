// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume;
    using Aupli.ApplicationServices.Volume.Api;
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Initialization;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Represents the domain logic module for volume.
    /// </summary>
    public class VolumeModule : IInitializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeModule" /> class.
        /// </summary>
        /// <param name="volumeRepository">The volume repository.</param>
        /// <param name="volumeIncrementStep">The volume increment step.</param>
        /// <param name="audioOutputStatusUpdater">The audio output status updater.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="volumeControl">The volume control.</param>
        /// <param name="volumeServiceReporter">The volume service reporter.</param>
        public VolumeModule(
            IAmplifier amplifier,
            IVolumeControl volumeControl,
            IAudioOutputStatusUpdater audioOutputStatusUpdater,
            IVolumeRepository volumeRepository,
            Percentage volumeIncrementStep,
            IVolumeServiceReporter volumeServiceReporter)
        {
            // Create application services
            this.VolumeService = new VolumeService(
                amplifier,
                volumeControl,
                audioOutputStatusUpdater,
                volumeRepository,
                new VolumeAdjuster(volumeIncrementStep),
                volumeServiceReporter);
        }

        /// <summary>
        /// Gets the volume service.
        /// </summary>
        /// <value>
        /// The volume service.
        /// </value>
        public IVolumeService VolumeService { get; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            await this.VolumeService.InitializeAsync().ConfigureAwait(false);
        }
    }
}