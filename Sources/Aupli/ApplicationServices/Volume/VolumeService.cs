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
    using Aupli.DomainServices.Interface.Volume;
    using Sundew.Base.Numeric;

    /// <summary>
    /// The volume service.
    /// </summary>
    /// <seealso cref="Aupli.ApplicationServices.RequiredInterface.Volume.IVolumeStatus" />
    /// <seealso cref="Aupli.ApplicationServices.Interface.IVolumeChangeNotifier" />
    public class VolumeService : IVolumeStatus, IVolumeChangeNotifier
    {
        private readonly IVolumeRepository volumeRepository;
        private readonly IVolumeAdjustmentService volumeAdjustmentService;
        private readonly IPlayerStatusUpdater playerStatusUpdater;
        private readonly IVolumeStatusUpdater volumeStatusUpdater;
        private readonly IAmplifier amplifier;
        private readonly IVolumeServiceReporter volumeServiceReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeService" /> class.
        /// </summary>
        /// <param name="volumeRepository">The volume repository.</param>
        /// <param name="volumeAdjustmentService">The volume service.</param>
        /// <param name="playerStatusUpdater">The player status.</param>
        /// <param name="volumeStatusUpdater">The volume status updater.</param>
        /// <param name="amplifier">The amplifier.</param>
        /// <param name="volumeServiceReporter">The volume controller reporter.</param>
        public VolumeService(
            IVolumeRepository volumeRepository,
            IVolumeAdjustmentService volumeAdjustmentService,
            IPlayerStatusUpdater playerStatusUpdater,
            IVolumeStatusUpdater volumeStatusUpdater,
            IAmplifier amplifier,
            IVolumeServiceReporter volumeServiceReporter)
        {
            this.volumeRepository = volumeRepository;
            this.volumeAdjustmentService = volumeAdjustmentService;
            this.playerStatusUpdater = playerStatusUpdater;
            this.volumeStatusUpdater = volumeStatusUpdater;
            this.amplifier = amplifier;
            this.volumeServiceReporter = volumeServiceReporter;
            this.volumeServiceReporter?.SetSource(this);
            this.playerStatusUpdater.StatusChanged += this.OnPlayerStatusUpdaterStatusChanged;
            this.volumeStatusUpdater.VolumeChanged += this.OnVolumeStatusUpdaterVolumeChanged;
            this.amplifier.SetVolume(Comparison.Min(this.volumeAdjustmentService.Volume.Percentage, new Percentage(0.8)));
            this.SetMuteState(this.playerStatusUpdater.Status.State);
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
        public Percentage Volume => this.volumeAdjustmentService.Volume.Percentage;

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted { get; private set; }

        /// <summary>
        /// Changes the volume asynchronous.
        /// </summary>
        /// <param name="isIncrementing">if set to <c>true</c> [is incrementing].</param>
        /// <returns>An async task.</returns>
        public async Task ChangeVolumeAsync(bool isIncrementing)
        {
            var newVolume = await this.volumeAdjustmentService.AdjustVolumeAsync(isIncrementing);
            this.volumeServiceReporter?.ChangeVolume(newVolume.Percentage);

            if (isIncrementing && this.amplifier.IsMuted)
            {
                this.IsMuted = false;
                this.amplifier.IsMuted = false;
            }

            this.amplifier.SetVolume(newVolume.Percentage);
            this.VolumeChanged?.Invoke(this, new VolumeEventArgs(newVolume.Percentage, this.IsMuted));
            this.volumeRepository.Volume = newVolume.Percentage;
            await this.volumeRepository.SaveAsync();
        }

        /// <summary>
        /// Toggles the mute.
        /// </summary>
        public void ToggleMute()
        {
            var newIsMuted = !this.IsMuted;
            var volume = this.volumeAdjustmentService.Volume;

            this.IsMuted = this.amplifier.IsMuted = newIsMuted;
            this.VolumeChanged?.Invoke(this, new VolumeEventArgs(volume.Percentage, this.IsMuted));

            this.volumeServiceReporter?.ChangeMute(newIsMuted);
        }

        private void OnPlayerStatusUpdaterStatusChanged(object sender, StatusEventArgs e)
        {
            if (!this.IsMuted)
            {
                this.SetMuteState(e.State);
            }
        }

        private void OnVolumeStatusUpdaterVolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            this.amplifier.SetVolume(e.Volume);
        }

        private void SetMuteState(PlayerState state)
        {
            this.amplifier.IsMuted = state != PlayerState.Playing;
        }
    }
}