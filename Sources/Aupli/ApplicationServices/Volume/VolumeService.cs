// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume
{
    using System;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume.Api;
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Numeric;

    /// <summary>
    /// The volume service.
    /// </summary>
    /// <seealso cref="IVolumeStatus" />
    /// <seealso cref="IVolumeChangeNotifier" />
    public class VolumeService : IVolumeService
    {
        private readonly VolumeAdjuster volumeAdjuster;
        private readonly IAmplifier amplifier;
        private readonly IAudioOutputStatusUpdater audioOutputStatusUpdater;
        private readonly IVolumeStatusUpdater volumeStatusUpdater;
        private readonly VolumeSynchronizerService volumeSynchronizerService;
        private readonly IVolumeServiceReporter volumeServiceReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeService" /> class.
        /// </summary>
        /// <param name="volumeAdjuster">The volume adjuster.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="audioOutputStatusUpdater">The player status.</param>
        /// <param name="volumeStatusUpdater">The volume status updater.</param>
        /// <param name="volumeSynchronizerService">The volume synchronizer service.</param>
        /// <param name="volumeServiceReporter">The volume controller reporter.</param>
        public VolumeService(
            VolumeAdjuster volumeAdjuster,
            IAmplifier amplifier,
            IAudioOutputStatusUpdater audioOutputStatusUpdater,
            IVolumeStatusUpdater volumeStatusUpdater,
            VolumeSynchronizerService volumeSynchronizerService,
            IVolumeServiceReporter volumeServiceReporter)
        {
            this.volumeAdjuster = volumeAdjuster;
            this.amplifier = amplifier;
            this.audioOutputStatusUpdater = audioOutputStatusUpdater;
            this.volumeStatusUpdater = volumeStatusUpdater;
            this.volumeSynchronizerService = volumeSynchronizerService;
            this.volumeServiceReporter = volumeServiceReporter;
            this.volumeServiceReporter?.SetSource(this);
            this.audioOutputStatusUpdater.AudioOutputStatusChanged += this.OnAudioOutputStatusUpdaterStatusChanged;
            this.volumeStatusUpdater.VolumeChanged += this.OnVolumeStatusUpdaterVolumeChanged;
        }

        /// <summary>
        /// Occurs when volume has changed.
        /// </summary>
        public event EventHandler VolumeChanged;

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume => this.volumeSynchronizerService.Volume;

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted => this.volumeSynchronizerService.IsMuted;

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            await this.volumeSynchronizerService.SetVolumeAsync(Comparison.Min(this.Volume, new Percentage(0.8)));
            this.SetAmplifierMuteState(this.audioOutputStatusUpdater.IsOutputtingAudio);
        }

        /// <summary>
        /// Changes the volume asynchronous.
        /// </summary>
        /// <param name="isIncrementing">if set to <c>true</c> [is incrementing].</param>
        /// <returns>An async task.</returns>
        public async Task ChangeVolumeAsync(bool isIncrementing)
        {
            var newVolume = this.volumeAdjuster.AdjustVolume(this.Volume, isIncrementing);
            await this.SetVolumeAsync(newVolume);
        }

        /// <summary>
        /// Toggles the mute.
        /// </summary>
        public async Task ToggleMuteAsync()
        {
            var newIsMuted = !this.IsMuted;

            await this.volumeSynchronizerService.SetMuteStateAsync(newIsMuted);
            this.VolumeChanged?.Invoke(this, EventArgs.Empty);

            this.volumeServiceReporter?.ChangeMute(newIsMuted);
        }

        private async Task SetVolumeAsync(Percentage newVolume)
        {
            if (this.Volume != newVolume)
            {
                this.volumeServiceReporter?.ChangeVolume(newVolume);
                if (this.Volume < newVolume && this.volumeSynchronizerService.IsMuted)
                {
                    await this.volumeSynchronizerService.SetMuteStateAsync(false);
                }

                await this.volumeSynchronizerService.SetVolumeAsync(newVolume);

                this.VolumeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnAudioOutputStatusUpdaterStatusChanged(object sender, EventArgs e)
        {
            if (!this.IsMuted)
            {
                this.SetAmplifierMuteState(this.audioOutputStatusUpdater.IsOutputtingAudio);
            }
        }

        private async void OnVolumeStatusUpdaterVolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            await this.SetVolumeAsync(e.Volume);
        }

        private void SetAmplifierMuteState(bool isOutputtingAudio)
        {
            this.amplifier.IsMuted = !isOutputtingAudio;
        }
    }
}