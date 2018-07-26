// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.RequiredInterface.Player
{
    using System;

    /// <summary>
    /// Music player status event arguments.
    /// </summary>
    /// <seealso cref="T:System.EventArgs" />
    public class StatusEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEventArgs" /> class.
        /// </summary>
        /// <param name="playerStatus">The player status.</param>
        public StatusEventArgs(PlayerStatus playerStatus)
        {
            this.PlayerStatus = playerStatus;
        }

        /// <summary>
        /// Gets the player status.
        /// </summary>
        /// <value>
        /// The player status.
        /// </value>
        public PlayerStatus PlayerStatus { get; }

        /// <summary>
        /// Gets the name of the playlist.
        /// </summary>
        /// <value>
        /// The name of the playlist.
        /// </value>
        public string PlaylistName => this.PlayerStatus.PlaylistName;

        /// <summary>
        /// Gets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist => this.PlayerStatus.Artist;

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title => this.PlayerStatus.Title;

        /// <summary>
        /// Gets the state of the player.
        /// </summary>
        /// <value>
        /// The state of the player.
        /// </value>
        public PlayerState State => this.PlayerStatus.State;

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <value>
        /// The track.
        /// </value>
        public int Track => this.PlayerStatus.Track;

        /// <summary>
        /// Gets the elapsed.
        /// </summary>
        /// <value>
        /// The elapsed.
        /// </value>
        public TimeSpan Elapsed => this.PlayerStatus.Elapsed;
    }
}