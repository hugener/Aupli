// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerStatus.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Mpc
{
    using System;

    /// <summary>
    /// Stores the status of a music player.
    /// </summary>
    public class PlayerStatus : IEquatable<PlayerStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStatus" /> class.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="playerState">Result of the player.</param>
        /// <param name="track">The track.</param>
        /// <param name="elapsed">The elapsed.</param>
        public PlayerStatus(string artist, string title, PlayerState playerState, int track, TimeSpan elapsed)
        {
            this.Artist = artist;
            this.Title = title;
            this.State = playerState;
            this.Track = track;
            this.Elapsed = elapsed;
        }

        /// <summary>
        /// Gets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public PlayerState State { get; }

        /// <summary>
        /// Gets the elapsed.
        /// </summary>
        /// <value>
        /// The elapsed.
        /// </value>
        public TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <value>
        /// The track.
        /// </value>
        public int Track { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(PlayerStatus other)
        {
            return this.Artist == other.Artist &&
                   this.Title == other.Title &&
                   this.State == other.State &&
                   this.Track == other.Track &&
                   this.Elapsed == other.Elapsed;
        }
    }
}