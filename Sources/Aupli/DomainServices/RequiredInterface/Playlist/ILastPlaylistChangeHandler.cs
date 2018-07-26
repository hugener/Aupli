// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILastPlaylistChangeHandler.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices.RequiredInterface.Playlist
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for handling last playlist changes.
    /// </summary>
    public interface ILastPlaylistChangeHandler
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<Shared.Playlist.PlaylistEntity> GetLastPlaylistAsync();

        /// <summary>
        /// Sets the last playlist.
        /// </summary>
        /// <param name="playlistEntity">The playlist.</param>
        /// <returns>An async task.</returns>
        Task SetLastPlaylistAsync(Shared.Playlist.PlaylistEntity playlistEntity);
    }
}