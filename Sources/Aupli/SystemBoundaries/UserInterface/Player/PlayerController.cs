// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Player
{
    using System;
    using Aupli.ApplicationServices.Interface.Player;
    using Aupli.ApplicationServices.Player;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Aupli.SystemBoundaries.Shared.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;

    /// <summary>
    /// Controls the music using player controls.
    /// </summary>
    public class PlayerController : IMenuRequester
    {
        private readonly IInteractionController interactionController;
        private readonly IPlayerService playerService;
        private readonly IPlaybackControls playbackControls;

        private readonly IPlayerControllerReporter playerControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController" /> class.
        /// </summary>
        /// <param name="interactionController">The input manager.</param>
        /// <param name="playerService">The player service.</param>
        /// <param name="playbackControls">The playback controls.</param>
        /// <param name="playerControllerReporter">The player controller reporter.</param>
        public PlayerController(
            IInteractionController interactionController,
            IPlayerService playerService,
            IPlaybackControls playbackControls,
            IPlayerControllerReporter playerControllerReporter)
        {
            this.interactionController = interactionController;
            this.playerService = playerService;
            this.playbackControls = playbackControls;
            this.playerControllerReporter = playerControllerReporter;
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
            this.interactionController.TagInputEvent.Register(this.OnInteractionControllerTagDetected);
            this.playerControllerReporter?.SetSource(this);
        }

        /// <summary>
        /// Occurs when the menu was requested.
        /// </summary>
        public event EventHandler MenuRequested;

        private async void OnInteractionControllerKeyInput(object sender, KeyInputArgs e)
        {
            this.playerControllerReporter.KeyInput(e.KeyInput);
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
            this.playerControllerReporter.TagInput(e);
            await this.playerService.StartPlaylistAsync(e.Uid).ConfigureAwait(false);
        }
    }
}