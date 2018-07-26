// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastPlaylistJsonFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Playlists
{
    using System.IO;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Playlist;
    using Aupli.DomainServices.Shared.Playlist;
    using Newtonsoft.Json;

    /// <summary>
    /// Repository for storing the last playlist.
    /// </summary>
    /// <seealso cref="ILastPlaylistRepository" />
    public class LastPlaylistJsonFileRepository : ILastPlaylistRepository
    {
        private readonly string filePath;
        private PlaylistEntity playlist;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastPlaylistJsonFileRepository"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public LastPlaylistJsonFileRepository(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task InitializeAsync()
        {
            this.playlist = JsonConvert.DeserializeObject<PlaylistEntity>(await File.ReadAllTextAsync(this.filePath));
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>
        /// The entity.
        /// </returns>
        public Task<PlaylistEntity> GetLastPlaylistAsync()
        {
            return Task.FromResult(this.playlist);
        }

        /// <summary>
        /// Sets the last playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        public void SetLastPlaylist(PlaylistEntity playlist)
        {
            this.playlist = playlist;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>
        /// A async task.
        /// </returns>
        public async Task SaveAsync()
        {
            await File.WriteAllTextAsync(this.filePath, JsonConvert.SerializeObject(this.playlist));
        }
    }
}