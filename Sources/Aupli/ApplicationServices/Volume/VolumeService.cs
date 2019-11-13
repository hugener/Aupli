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
    using Sundew.Base.Initialization;
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
        private readonly IVolumeControl volumeControl;
        private readonly IAudioOutputStatusUpdater audioOutputStatusUpdater;
        private readonly IVolumeRepository volumeRepository;
        private readonly IVolumeServiceReporter? volumeServiceReporter;
        private readonly InitializeFlag initializeFlag = new InitializeFlag();

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeService" /> class.
        /// </summary>
        /// <param name="volumeAdjuster">The volume adjuster.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="audioOutputStatusUpdater">The player status.</param>
        /// <param name="volumeControl">The volume control.</param>
        /// <param name="volumeRepository">The volume repository.</param>
        /// <param name="volumeServiceReporter">The volume controller reporter.</param>
        public VolumeService(
            IAmplifier amplifier,
            IVolumeControl volumeControl,
            IAudioOutputStatusUpdater audioOutputStatusUpdater,
            IVolumeRepository volumeRepository,
            VolumeAdjuster volumeAdjuster,
            IVolumeServiceReporter? volumeServiceReporter)
        {
            this.volumeAdjuster = volumeAdjuster;
            this.amplifier = amplifier;
            this.volumeControl = volumeControl;
            this.audioOutputStatusUpdater = audioOutputStatusUpdater;
            this.volumeRepository = volumeRepository;
            this.volumeServiceReporter = volumeServiceReporter;
            this.volumeServiceReporter?.SetSource(this);
            this.audioOutputStatusUpdater.AudioOutputStatusChanged += this.OnAudioOutputStatusUpdaterStatusChanged;
        }

        /// <summary>
        /// Occurs when volume has changed.
        /// </summary>
        public event EventHandler? VolumeChanged;

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
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async ValueTask InitializeAsync()
        {
            await this.SetVolumeAllAsync(Comparison.Min(this.Volume, new Percentage(0.8)));
            this.SetAmplifierMuteState(this.audioOutputStatusUpdater.IsAudioOutputActive);
            this.volumeControl.VolumeChanged += this.OnVolumeStatusUpdaterVolumeChanged;
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
            this.volumeServiceReporter?.ChangeMute(newIsMuted);
            await this.SetMuteStateAsync(newIsMuted);
            this.VolumeChanged?.Invoke(this, EventArgs.Empty);
        }

        private async Task SetVolumeAsync(Percentage newVolume)
        {
            if (this.Volume != newVolume)
            {
                this.volumeServiceReporter?.ChangeVolume(newVolume);
                if (this.Volume < newVolume && this.IsMuted)
                {
                    if (this.audioOutputStatusUpdater.IsAudioOutputActive)
                    {
                        await this.SetMuteStateAsync(false).ConfigureAwait(false);
                    }
                    else
                    {
                        this.IsMuted = false;
                    }
                }

                await this.SetVolumeAllAsync(newVolume).ConfigureAwait(false);
                this.VolumeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnAudioOutputStatusUpdaterStatusChanged(object? sender, EventArgs e)
        {
            if (!this.IsMuted)
            {
                this.SetAmplifierMuteState(this.audioOutputStatusUpdater.IsAudioOutputActive);
            }
        }

        private async void OnVolumeStatusUpdaterVolumeChanged(object? sender, VolumeChangedEventArgs e)
        {
            if (this.initializeFlag.Initialize())
            {
                return;
            }

            if (!e.IsMuted)
            {
                await this.SetVolumeAsync(e.Volume);
            }
            else
            {
                await this.SetMuteStateAsync(true);
            }
        }

        private void SetAmplifierMuteState(bool isAudioOutputActive)
        {
            this.amplifier.IsMuted = !isAudioOutputActive;
        }

        private async Task SetVolumeAllAsync(Percentage volume)
        {
            this.volumeRepository.Volume = volume;
            this.amplifier.SetVolume(volume);
            await this.volumeControl.SetVolumeAsync(volume);
            await this.volumeRepository.SaveAsync();
        }

        private async Task SetMuteStateAsync(bool isMuted)
        {
            this.IsMuted = isMuted;
            this.amplifier.IsMuted = isMuted;
            if (isMuted)
            {
                await this.volumeControl.MuteAsync();
            }
            else
            {
                await this.volumeControl.SetVolumeAsync(this.volumeRepository.Volume);
            }
        }
    }
}