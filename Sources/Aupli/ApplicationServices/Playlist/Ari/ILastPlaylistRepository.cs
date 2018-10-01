// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILastPlaylistRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Playlist.Ari
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Playlist.Shared;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Interface for persisting the last playlist.
    /// </summary>
    public interface ILastPlaylistRepository : IInitializable
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<PlaylistEntity> GetLastPlaylistAsync();

        /// <summary>
        /// Sets the last playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        void SetLastPlaylist(PlaylistEntity playlist);

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        Task SaveAsync();
    }
}