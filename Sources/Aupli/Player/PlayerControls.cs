// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerControls.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Player
{
    using Sundew.Pi.IO.Components.Buttons;
    using Sundew.Pi.IO.Components.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Components.RfidTransceivers.Mfrc522;

    /// <summary>
    /// Hardware controls related to playback.
    /// </summary>
    public class PlayerControls
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerControls"/> class.
        /// </summary>
        /// <param name="playPauseButton">The play pause button.</param>
        /// <param name="nextButton">The next button.</param>
        /// <param name="previousButton">The previous button.</param>
        /// <param name="mfrc522Connection">The MFRC522 connection.</param>
        /// <param name="lircConnection">The lirc connection.</param>
        public PlayerControls(
            PullDownButtonConnection playPauseButton,
            PullDownButtonConnection nextButton,
            PullDownButtonConnection previousButton,
            Mfrc522Connection mfrc522Connection,
            LircConnection lircConnection)
        {
            this.PlayPauseButton = playPauseButton;
            this.NextButton = nextButton;
            this.PreviousButton = previousButton;
            this.Mfrc522Connection = mfrc522Connection;
            this.LircConnection = lircConnection;
        }

        /// <summary>
        /// Gets the play pause button.
        /// </summary>
        /// <value>
        /// The play pause button.
        /// </value>
        public PullDownButtonConnection PlayPauseButton { get; }

        /// <summary>
        /// Gets the next button.
        /// </summary>
        /// <value>
        /// The next button.
        /// </value>
        public PullDownButtonConnection NextButton { get; }

        /// <summary>
        /// Gets the previous button.
        /// </summary>
        /// <value>
        /// The previous button.
        /// </value>
        public PullDownButtonConnection PreviousButton { get; }

        /// <summary>
        /// Gets the MFRC522 connection.
        /// </summary>
        /// <value>
        /// The MFRC522 connection.
        /// </value>
        public Mfrc522Connection Mfrc522Connection { get; }

        /// <summary>
        /// Gets or sets the lirc connection.
        /// </summary>
        /// <value>
        /// The lirc connection.
        /// </value>
        public LircConnection LircConnection { get; set; }
    }
}