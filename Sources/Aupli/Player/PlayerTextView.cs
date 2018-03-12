// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Player
{
    using System;
    using Aupli.Mpc;
    using Aupli.Volume;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;
    using Sundew.Pi.ApplicationFramework.TextViewRendering.Animation;

    /// <summary>
    /// A textView for displaying information from the music player.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class PlayerTextView : ITextView
    {
        private static readonly TimeSpan AnimationStartDelay = TimeSpan.FromMilliseconds(3000);
        private static readonly TimeSpan AnimationInterval = TimeSpan.FromMilliseconds(1000);
        private static readonly TimeSpan AnimationPauseDelay = TimeSpan.FromMilliseconds(3000);
        private readonly IPlayerInfo playerInfo;
        private readonly IVolumeInfo volumeInfo;
        private PlayerStatus playerStatus;
        private PlayerStatus previousPlayerStatus;
        private IInvalidater invalidater;
        private TextScroller artistTextScroller;
        private TextScroller titleTextScroller;
        private TextBlinker muteTextBlinker;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextView" /> class.
        /// </summary>
        /// <param name="playerInfo">The player information.</param>
        /// <param name="volumeInfo">The volume information.</param>
        public PlayerTextView(IPlayerInfo playerInfo, IVolumeInfo volumeInfo)
        {
            this.playerInfo = playerInfo;
            this.volumeInfo = volumeInfo;
            this.ResetPlayerState();
        }

        /// <summary>
        /// Called before the text view is shown.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        public async void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.ResetPlayerState();
            PlayerCustomCharacters.SetCharacters(characterContext);
            this.invalidater = invalidater;
            this.playerStatus = this.playerInfo.Status;
            this.playerInfo.StatusChanged += this.OnPlayerInfoStatusChanged;
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
            await this.playerInfo.UpdateStatusAsync();
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
                renderContext.SetPosition(renderContext.Width - 5, 0);
                renderContext.Write(this.GetElapsed());
            }
            else
            {
                renderContext.Home();
                if (this.playerStatus.State == PlayerState.Unknown || this.playerStatus.State == PlayerState.Stopped)
                {
                    renderContext.WriteLine("Hold card above");
                    renderContext.WriteLine("to start music");
                    return;
                }

                renderContext.WriteLine(
                    $"{this.artistTextScroller.GetFrame(this.playerStatus.Artist, renderContext.Width - 6, 0)} {this.GetElapsed()}");
                var trackText = $" #{this.playerStatus.Track + 1:D2}";
                renderContext.WriteLine(
                    $"{this.titleTextScroller.GetFrame(this.playerStatus.Title, renderContext.Width - trackText.Length, 0)}{trackText}");
            }

            if (this.playerStatus.State == PlayerState.Paused)
            {
                renderContext.SetPosition(6, 0);
                renderContext.WriteLine(PlayerCustomCharacters.Pause);
            }
        }

        /// <inheritdoc />
        public void OnClosing()
        {
            this.playerInfo.StatusChanged -= this.OnPlayerInfoStatusChanged;
        }

        private void OnPlayerInfoStatusChanged(object sender, StatusEventArgs e)
        {
            this.playerStatus = new PlayerStatus(e.Artist, e.Title, e.State, e.Track, e.Elapsed);
            this.invalidater?.Invalidate();
        }

        private string GetElapsed()
        {
            var muteText = this.volumeInfo.IsMuted
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