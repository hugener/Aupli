// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Player
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Volume.Api;
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;
    using Sundew.Pi.ApplicationFramework.TextViewRendering.Animation;

    /// <summary>
    /// A textView for displaying information from the music player.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class PlayerTextView : ITextView
    {
        private static readonly TimeSpan AnimationStartDelay = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan AnimationInterval = TimeSpan.FromMilliseconds(900);
        private static readonly TimeSpan AnimationPauseDelay = TimeSpan.FromMilliseconds(4000);
        private readonly PlayerController playerController;
        private readonly IPlayerStatusUpdater playerStatusUpdater;
        private readonly IVolumeStatus volumeStatus;
        private PlayerStatus playerStatus;
        private PlayerStatus previousPlayerStatus;
        private IInvalidater invalidater;
        private TextScroller artistTextScroller;
        private TextScroller titleTextScroller;
        private TextBlinker muteTextBlinker;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextView" /> class.
        /// </summary>
        /// <param name="playerController">The player controller.</param>
        /// <param name="playerStatusUpdater">The player information.</param>
        /// <param name="volumeStatus">The volume status.</param>
        public PlayerTextView(PlayerController playerController, IPlayerStatusUpdater playerStatusUpdater, IVolumeStatus volumeStatus)
        {
            this.playerController = playerController;
            this.playerStatusUpdater = playerStatusUpdater;
            this.volumeStatus = volumeStatus;
            this.ResetPlayerState();
        }

        /// <summary>
        /// Gets the input targets.
        /// </summary>
        /// <value>
        /// The input targets.
        /// </value>
        public IEnumerable<object> InputTargets => new[] { this.playerController };

        /// <summary>
        /// Called before the text view is shown.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        public async Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.ResetPlayerState();
            PlayerCustomCharacters.SetCharacters(characterContext);
            this.invalidater = invalidater;
            this.playerStatus = this.playerStatusUpdater.Status;
            this.playerStatusUpdater.StatusChanged += this.OnPlayerStatusUpdaterStatusChanged;
            this.artistTextScroller = new TextScroller(
                invalidater,
                ScrollMode.Restart,
                AnimationStartDelay,
                AnimationInterval,
                AnimationPauseDelay);
            this.titleTextScroller = new TextScroller(
                invalidater,
                ScrollMode.Restart,
                AnimationStartDelay,
                AnimationInterval,
                AnimationPauseDelay);
            this.muteTextBlinker = new TextBlinker(invalidater, AnimationInterval);
            await this.playerStatusUpdater.UpdateStatusAsync();
        }

        /// <summary>
        /// Renders the player text view.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            var localPreviousPlayerStatus = this.previousPlayerStatus;
            this.previousPlayerStatus = this.playerStatus;
            if (this.ShouldUpdateElapsedOnly(localPreviousPlayerStatus))
            {
                renderContext.SetPosition(renderContext.Size.Width - 5, 0);
                renderContext.Write(this.GetElapsed());
            }
            else
            {
                renderContext.SetPosition(0, 0);
                if (this.playerStatus.State == PlayerState.Unknown || this.playerStatus.State == PlayerState.Stopped)
                {
                    renderContext.WriteLine("Hold tag above".LimitAndPadRight(renderContext.Size.Width, ' '));
                    renderContext.WriteLine("to start music".LimitAndPadRight(renderContext.Size.Width, ' '));
                    return;
                }

                var artistAndTrack = $"{this.artistTextScroller.GetFrame(this.playerStatus.Artist, renderContext.Size.Width - 6, 0)} {this.GetElapsed()}";
                if (this.playerStatus.State == PlayerState.Paused)
                {
                    artistAndTrack = artistAndTrack.ReplaceAt(6, PlayerCustomCharacters.Pause);
                }

                renderContext.WriteLine(artistAndTrack);

                var trackText = $" #{this.playerStatus.Track + 1:D2}";
                renderContext.WriteLine(
                    $"{this.titleTextScroller.GetFrame(this.playerStatus.Title, renderContext.Size.Width - trackText.Length, 0)}{trackText}");
            }
        }

        /// <inheritdoc />
        public Task OnClosingAsync()
        {
            this.playerStatusUpdater.StatusChanged -= this.OnPlayerStatusUpdaterStatusChanged;
            return Task.CompletedTask;
        }

        private void OnPlayerStatusUpdaterStatusChanged(object sender, StatusEventArgs e)
        {
            this.playerStatus = new PlayerStatus(e.PlaylistName, e.Artist, e.Title, e.State, e.Track, e.Elapsed);
            this.invalidater?.Invalidate();
        }

        private string GetElapsed()
        {
            var muteText = this.volumeStatus.IsMuted
                ? this.muteTextBlinker.GetFrame(PlayerCustomCharacters.Mute, 0, 0)
                : ":";
            return $"{this.playerStatus.Elapsed:mm}{muteText}{this.playerStatus.Elapsed:ss}";
        }

        private void ResetPlayerState()
        {
            this.playerStatus = PlayerStatus.NoStatus;
            this.previousPlayerStatus = null;
        }

        private bool ShouldUpdateElapsedOnly(PlayerStatus localPreviousPlayerStatus)
        {
            return localPreviousPlayerStatus != null &&
                   !this.artistTextScroller.IsChanged &&
                   !this.titleTextScroller.IsChanged &&
                   localPreviousPlayerStatus.Artist == this.playerStatus.Artist &&
                   localPreviousPlayerStatus.Title == this.playerStatus.Title &&
                   localPreviousPlayerStatus.Track == this.playerStatus.Track &&
                   localPreviousPlayerStatus.State == this.playerStatus.State;
        }
    }
}