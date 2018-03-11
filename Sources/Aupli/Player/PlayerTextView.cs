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
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// A textView for displaying information from the music player.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class PlayerTextView : ITextView
    {
        private readonly IPlayerInfo playerInfo;
        private PlayerStatus playerStatus;
        private PlayerStatus previousPlayerStatus;
        private IInvalidater invalidater;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTextView" /> class.
        /// </summary>
        /// <param name="playerInfo">The player information.</param>
        public PlayerTextView(IPlayerInfo playerInfo)
        {
            this.playerInfo = playerInfo;
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
            if (localPreviousPlayerStatus.Artist == this.playerStatus.Artist &&
                localPreviousPlayerStatus.Title == this.playerStatus.Title &&
                localPreviousPlayerStatus.Track == this.playerStatus.Track &&
                localPreviousPlayerStatus.State == this.playerStatus.State)
            {
                renderContext.SetPosition(renderContext.Width - 5, 0);
                renderContext.Write($"{this.playerStatus.Elapsed:mm\\:ss}");
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
                    $"{this.playerStatus.Artist.LimitAndPadRight(renderContext.Width - 6, ' ')} {this.playerStatus.Elapsed:mm\\:ss}");
                var trackText = $" #{this.playerStatus.Track + 1:D2}";
                renderContext.WriteLine(
                    $"{this.playerStatus.Title.LimitAndPadRight(renderContext.Width - trackText.Length, ' ')}{trackText}");
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

        private void ResetPlayerState()
        {
            this.playerStatus = PlayerStatus.NoStatus;
            this.previousPlayerStatus = PlayerStatus.NoStatus;
        }
    }
}