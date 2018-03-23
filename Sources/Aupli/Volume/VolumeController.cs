// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using System;
    using System.Threading.Tasks;
    using Aupli.Input;
    using Aupli.Mpc;
    using Aupli.Numeric;
    using Serilog;
    using Sundew.Pi.IO.Devices.Buttons;

    /// <summary>
    /// Controls the volume of the MAX 9744 using the KY-040.
    /// </summary>
    public class VolumeController : IVolumeInfo
    {
        private readonly IVolumeControl volumeControl;
        private readonly IPlayerInfo playerInfo;
        private readonly InteractionController interactionController;
        private readonly VolumeControls volumeControls;
        private readonly IVolumeSettings volumeSettings;
        private readonly ILogger log;
        private bool isUsingHeadPhones;
        private bool isMuted;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeController" /> class.
        /// </summary>
        /// <param name="volumeControl">The music player.</param>
        /// <param name="playerInfo">The player status.</param>
        /// <param name="interactionController">The interaction controller.</param>
        /// <param name="volumeControls">The volume controls.</param>
        /// <param name="volumeSettings">The volume settings.</param>
        /// <param name="logger">The logger.</param>
        public VolumeController(
            IVolumeControl volumeControl,
            IPlayerInfo playerInfo,
            InteractionController interactionController,
            VolumeControls volumeControls,
            IVolumeSettings volumeSettings,
            ILogger logger)
        {
            this.volumeControl = volumeControl;
            this.playerInfo = playerInfo;
            this.interactionController = interactionController;
            this.volumeControls = volumeControls;
            this.volumeSettings = volumeSettings;
            this.log = logger.ForContext<VolumeController>();
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
            this.playerInfo.StatusChanged += this.OnMusicPlayerInfoChanged;
        }

        /// <summary>
        /// Occurs when volume has changed.
        /// </summary>
        public event EventHandler<VolumeEventArgs> VolumeChanged;

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume => this.volumeControls.VolumeAdjuster.Volume.Percentage;

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted => this.isMuted;

        /// <summary>
        /// Starts the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task StartAsync()
        {
            if (this.volumeControls.HeadPhoneSwitch != null)
            {
                this.volumeControls.HeadPhoneSwitch.StateChanged += this.OnHeadPhoneSwitched;
                await this.UpdateOutputAsync(this.volumeControls.HeadPhoneSwitch.State);
            }
            else
            {
                this.volumeControls.Amplifier.SetVolume(this.volumeControls.VolumeAdjuster.Volume.AbsoluteValue);
            }

            this.log.Debug("Started");
        }

        private async void OnInteractionControllerKeyInput(object sender, KeyInputArgs e)
        {
            switch (e.KeyInput)
            {
                case KeyInput.Down:
                    await this.ChangeVolumeAsync(false);
                    break;
                case KeyInput.Up:
                    await this.ChangeVolumeAsync(true);
                    break;
                case KeyInput.Select:
                    await this.ToggleMuteAsync();
                    break;
            }
        }

        private async Task ChangeVolumeAsync(bool isIncrementing)
        {
            var newVolume = this.volumeControls.VolumeAdjuster.Adjust(isIncrementing);
            this.log.Debug("Change volume: {NewVolume}", newVolume.Percentage);
            if (this.isUsingHeadPhones)
            {
                await this.volumeControl.SetVolumeAsync(newVolume.Percentage);
                return;
            }

            if (isIncrementing && this.volumeControls.Amplifier.IsMuted)
            {
                this.isMuted = false;
                this.volumeControls.Amplifier.SetMuteState(false);
            }

            this.volumeControls.Amplifier.SetVolume(newVolume.AbsoluteValue);
            this.volumeSettings.Volume = newVolume.Percentage.Value;
            this.VolumeChanged?.Invoke(this, new VolumeEventArgs(newVolume.Percentage, this.isMuted));
        }

        private async Task ToggleMuteAsync()
        {
            var newIsMuted = !this.isMuted;
            var volume = this.volumeControls.VolumeAdjuster.Volume;
            if (this.isUsingHeadPhones)
            {
                this.isMuted = newIsMuted;
                await this.volumeControl.SetVolumeAsync(this.isMuted ? new Percentage(0) : volume.Percentage);
            }
            else
            {
                this.isMuted = this.volumeControls.Amplifier.SetMuteState(newIsMuted);
                this.VolumeChanged?.Invoke(this, new VolumeEventArgs(volume.Percentage, this.isMuted));
            }

            this.log.Debug(newIsMuted ? "Muted" : "Unmuted");
        }

        private async Task UpdateOutputAsync(bool isUsingHeadPhones)
        {
            var volume = this.volumeControls.VolumeAdjuster.Volume;
            this.isUsingHeadPhones = this.volumeControls.Amplifier.SetMuteState(isUsingHeadPhones);
            if (this.isUsingHeadPhones)
            {
                await this.volumeControl.SetVolumeAsync(volume.Percentage);
            }
            else
            {
                await this.volumeControl.SetVolumeAsync(new Percentage(1));
            }
        }

        private async void OnHeadPhoneSwitched(object sender, SwitchEventArgs e)
        {
            await this.UpdateOutputAsync(e.State);
        }

        private void OnMusicPlayerInfoChanged(object sender, StatusEventArgs e)
        {
            if (!this.isMuted)
            {
                this.volumeControls.Amplifier.SetMuteState(e.State != PlayerState.Playing || this.isUsingHeadPhones);
            }
        }
    }
}
