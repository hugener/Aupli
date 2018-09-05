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
    using Aupli.ApplicationServices.Interface;
    using Aupli.ApplicationServices.RequiredInterface.Amplifier;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Aupli.ApplicationServices.RequiredInterface.Volume;
    using Sundew.Base.Initialization;
    using Sundew.Base.Numeric;

    /// <summary>
    /// The volume service.
    /// </summary>
    /// <seealso cref="Aupli.ApplicationServices.RequiredInterface.Volume.IVolumeStatus" />
    /// <seealso cref="Aupli.ApplicationServices.Interface.IVolumeChangeNotifier" />
    public class VolumeService : IVolumeStatus, IVolumeChangeNotifier, IInitializable
    {
        private readonly VolumeAdjuster volumeAdjuster;
        private readonly IAmplifier amplifier;
        private readonly IPlayerStatusUpdater playerStatusUpdater;
        private readonly IVolumeStatusUpdater volumeStatusUpdater;
        private readonly VolumeSynchronizerService volumeSynchronizerService;
        private readonly IVolumeServiceReporter volumeServiceReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeService" /> class.
        /// </summary>
        /// <param name="volumeAdjuster">The volume adjuster.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="playerStatusUpdater">The player status.</param>
        /// <param name="volumeStatusUpdater">The volume status updater.</param>
        /// <param name="volumeSynchronizerService">The volume synchronizer service.</param>
        /// <param name="volumeServiceReporter">The volume controller reporter.</param>
        public VolumeService(
            VolumeAdjuster volumeAdjuster,
            IAmplifier amplifier,
            IPlayerStatusUpdater playerStatusUpdater,
            IVolumeStatusUpdater volumeStatusUpdater,
            VolumeSynchronizerService volumeSynchronizerService,
            IVolumeServiceReporter volumeServiceReporter)
        {
            this.volumeAdjuster = volumeAdjuster;
            this.amplifier = amplifier;
            this.playerStatusUpdater = playerStatusUpdater;
            this.volumeStatusUpdater = volumeStatusUpdater;
            this.volumeSynchronizerService = volumeSynchronizerService;
            this.volumeServiceReporter = volumeServiceReporter;
            this.volumeServiceReporter?.SetSource(this);
            this.playerStatusUpdater.StatusChanged += this.OnPlayerStatusUpdaterStatusChanged;
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
            this.SetAmplifierMuteState(this.playerStatusUpdater.Status.State);
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
            this.VolumeChanged?.Invoke(this, new VolumeEventArgs(this.Volume, this.IsMuted));

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

                this.VolumeChanged?.Invoke(this, new VolumeEventArgs(newVolume, this.IsMuted));
            }
        }

        private void OnPlayerStatusUpdaterStatusChanged(object sender, StatusEventArgs e)
        {
            if (!this.IsMuted)
            {
                this.SetAmplifierMuteState(e.State);
            }
        }

        private async void OnVolumeStatusUpdaterVolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            await this.SetVolumeAsync(e.Volume);
        }

        private void SetAmplifierMuteState(PlayerState state)
        {
            this.amplifier.IsMuted = state != PlayerState.Playing;
        }
    }
}