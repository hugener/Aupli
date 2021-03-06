﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Player
{
    using Aupli.ApplicationServices.Player.Api;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Player.Ari;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Controls the music using player controls.
    /// </summary>
    public class PlayerController
    {
        private readonly IInteractionController interactionController;
        private readonly IPlayerService playerService;
        private readonly IPlaybackControls playbackControls;
        private readonly IMenuRequester menuRequester;

        private readonly IPlayerControllerReporter? playerControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController" /> class.
        /// </summary>
        /// <param name="interactionController">The input manager.</param>
        /// <param name="playerService">The player service.</param>
        /// <param name="playbackControls">The playback controls.</param>
        /// <param name="menuRequester">The menu requester.</param>
        /// <param name="playerControllerReporter">The player controller reporter.</param>
        public PlayerController(
            IInteractionController interactionController,
            IPlayerService playerService,
            IPlaybackControls playbackControls,
            IMenuRequester menuRequester,
            IPlayerControllerReporter? playerControllerReporter)
        {
            this.interactionController = interactionController;
            this.playerService = playerService;
            this.playbackControls = playbackControls;
            this.menuRequester = menuRequester;
            this.playerControllerReporter = playerControllerReporter;
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
            this.interactionController.TagInputEvent.Register(this.OnInteractionControllerTagDetected);
            this.playerControllerReporter?.SetSource(this);
        }

        private void OnInteractionControllerKeyInput(object? sender, KeyInputArgs e)
        {
            this.playerControllerReporter?.KeyInput(e.KeyInput);
            switch (e.KeyInput)
            {
                case KeyInput.PlayPause:
                case KeyInput.Ok:
                case KeyInput.Stop:
                    this.playbackControls.PauseResumeAsync();
                    break;
                case KeyInput.Next:
                case KeyInput.Right:
                    this.playbackControls.NextAsync();
                    break;
                case KeyInput.Previous:
                case KeyInput.Left:
                    this.playbackControls.PreviousAsync();
                    break;
                case KeyInput.Menu:
                    this.menuRequester.RequestMenu();
                    break;
            }
        }

        private void OnInteractionControllerTagDetected(object? sender, TagInputArgs e)
        {
            this.playerControllerReporter?.TagInput(e);
            this.playerService.StartPlaylistAsync(e.Uid);
        }
    }
}