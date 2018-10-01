// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILastPlaylistService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices.Playlist.Api
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Playlist.Shared;

    /// <summary>
    /// Maintains the last play list.
    /// </summary>
    public interface ILastPlaylistService
    {
        /// <summary>
        /// Gets the last playlist.
        /// </summary>
        /// <value>
        /// The last playlist.
        /// </value>
        PlaylistEntity LastPlaylist { get; }

        /// <summary>
        /// Changes the last playlist.
        /// </summary>
        /// <param name="playlistEntity">The playlist.</param>
        /// <returns>An async task.</returns>
        Task ChangeLastPlaylistAsync(PlaylistEntity playlistEntity);
    }
}