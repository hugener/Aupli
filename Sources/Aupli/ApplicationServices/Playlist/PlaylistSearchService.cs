// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaylistSearchService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Playlist
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Playlist;
    using Aupli.DomainServices.Interface.Playlist;

    /// <summary>
    /// Finds a playlist based on an id.
    /// </summary>
    public class PlaylistSearchService : IPlaylistSearchService
    {
        private readonly IPlaylistRepository playlistRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistSearchService"/> class.
        /// </summary>
        /// <param name="playlistRepository">The playlist repository.</param>
        public PlaylistSearchService(IPlaylistRepository playlistRepository)
        {
            this.playlistRepository = playlistRepository;
        }

        /// <summary>
        /// Gets the playlist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The playlist name.
        /// </returns>
        public Task<DomainServices.Shared.Playlist.PlaylistEntity> GetPlaylistAsync(string id)
        {
            return this.playlistRepository.GetPlaylistAsync(id);
        }
    }
}