// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Player
{
    using System;
    using System.Threading.Tasks;
    using Aupli.Input;
    using Aupli.Mpc;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Controls the music using player controls.
    /// </summary>
    public class PlayerController
    {
        private readonly InteractionController interactionController;
        private readonly IPlaybackControls playbackControls;
        private readonly PlaylistMap playlistMap;
        private readonly IPlayerSettings playerSettings;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController" /> class.
        /// </summary>
        /// <param name="interactionController">The input manager.</param>
        /// <param name="playbackControls">The music player.</param>
        /// <param name="playlistMap">The playlist map.</param>
        /// <param name="playerSettings">The player settings.</param>
        /// <param name="log">The log.</param>
        public PlayerController(
            InteractionController interactionController,
            IPlaybackControls playbackControls,
            PlaylistMap playlistMap,
            IPlayerSettings playerSettings,
            ILog log)
        {
            this.interactionController = interactionController;
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
            this.interactionController.TagInputEvent.Register(this.OnInteractionControllerTagDetected);
            this.playbackControls = playbackControls;
            this.playlistMap = playlistMap;
            this.playerSettings = playerSettings;
            this.logger = log.GetCategorizedLogger(typeof(PlayerController), true);
        }

        /// <summary>
        /// Occurs when the menu was requested.
        /// </summary>
        public event EventHandler MenuRequested;

        /// <summary>
        /// Starts the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task StartAsync()
        {
            await this.playbackControls.UpdateAsync();
            if (this.playbackControls.Status.State != PlayerState.Playing && !string.IsNullOrEmpty(this.playerSettings.LastPlaylist))
            {
                await this.playbackControls.PlayPlaylistAsync(this.playerSettings.LastPlaylist);
            }
        }

        private async void OnInteractionControllerKeyInput(object sender, KeyInputArgs e)
        {
            switch (e.KeyInput)
            {
                case KeyInput.PlayPause:
                case KeyInput.Ok:
                case KeyInput.Stop:
                    await this.playbackControls.PlayPauseAsync();
                    break;
                case KeyInput.Next:
                case KeyInput.Right:
                    await this.playbackControls.NextAsync();
                    break;
                case KeyInput.Previous:
                case KeyInput.Left:
                    await this.playbackControls.PreviousAsync();
                    break;
                case KeyInput.Menu:
                    this.MenuRequested?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private async void OnInteractionControllerTagDetected(object sender, TagInputArgs e)
        {
            var playlist = this.playlistMap.GetPlaylist(e.Uid);
            if (playlist != null)
            {
                this.logger.LogInfo("Found playlist: " + playlist);
            }

            await this.playbackControls.PlayPlaylistAsync(playlist);
            if (string.IsNullOrEmpty(playlist))
            {
                this.playerSettings.LastPlaylist = playlist;
            }
        }
    }
}