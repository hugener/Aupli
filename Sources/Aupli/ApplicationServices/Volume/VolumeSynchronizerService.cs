// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeSynchronizerService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Service for synchronizing the volume across volume controls.
    /// </summary>
    public class VolumeSynchronizerService
    {
        private readonly IAmplifier amplifier;
        private readonly IVolumeControl volumeControl;
        private readonly IVolumeRepository volumeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeSynchronizerService" /> class.
        /// </summary>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="volumeControl">The volume control.</param>
        /// <param name="volumeRepository">The volume repository.</param>
        public VolumeSynchronizerService(IAmplifier amplifier, IVolumeControl volumeControl, IVolumeRepository volumeRepository)
        {
            this.amplifier = amplifier;
            this.volumeControl = volumeControl;
            this.volumeRepository = volumeRepository;
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume => this.volumeRepository.Volume;

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted { get; private set; }

        /// <summary>
        /// Sets the volume asynchronous.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <returns>An async task.</returns>
        public async Task SetVolumeAsync(Percentage volume)
        {
            this.amplifier.SetVolume(volume);
            await this.volumeControl.SetVolumeAsync(volume);
            this.volumeRepository.Volume = volume;
            await this.volumeRepository.SaveAsync();
        }

        /// <summary>
        /// Sets the mute state asynchronous.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        /// <returns>An async task.</returns>
        public async Task SetMuteStateAsync(bool isMuted)
        {
            this.IsMuted = isMuted;
            this.amplifier.IsMuted = isMuted;
            await this.volumeControl.SetMuteStateAsync(isMuted);
        }
    }
}