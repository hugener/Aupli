// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastPlaylistChangeHandler.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Playlist
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Playlist.Ari;
    using Aupli.DomainServices.Playlist.Ari;
    using Aupli.DomainServices.Playlist.Shared;

    /// <summary>
    /// Handles changes to the last playlist.
    /// </summary>
    /// <seealso cref="ILastPlaylistChangeHandler" />
    public class LastPlaylistChangeHandler : ILastPlaylistChangeHandler
    {
        private readonly ILastPlaylistRepository lastPlaylistRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastPlaylistChangeHandler"/> class.
        /// </summary>
        /// <param name="lastPlaylistRepository">The last playlist repository.</param>
        public LastPlaylistChangeHandler(ILastPlaylistRepository lastPlaylistRepository)
        {
            this.lastPlaylistRepository = lastPlaylistRepository;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>
        /// The entity.
        /// </returns>
        public async Task<PlaylistEntity> GetLastPlaylistAsync()
        {
            return await this.lastPlaylistRepository.GetLastPlaylistAsync();
        }

        /// <summary>
        /// Sets the last playlist.
        /// </summary>
        /// <param name="playlistEntity">The playlist.</param>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task SetLastPlaylistAsync(PlaylistEntity playlistEntity)
        {
            this.lastPlaylistRepository.SetLastPlaylist(playlistEntity);
            await this.lastPlaylistRepository.SaveAsync();
        }
    }
}