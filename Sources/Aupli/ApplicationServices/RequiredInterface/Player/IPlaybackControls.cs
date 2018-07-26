// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlaybackControls.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.RequiredInterface.Player
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing playback controls.
    /// </summary>
    public interface IPlaybackControls : IPlayerStatusUpdater
    {
        /// <summary>
        /// Updates the databse asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        Task UpdateAsync();

        /// <summary>
        /// Plays the playlist asynchronous.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <returns>An async task.</returns>
        Task PlayPlaylistAsync(string playlistName);

        /// <summary>
        /// Plays or pauses asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        Task PlayPauseAsync();

        /// <summary>
        /// Plays the next song asynchronously.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        Task NextAsync();

        /// <summary>
        /// Plays the previous song asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        Task PreviousAsync();
    }
}