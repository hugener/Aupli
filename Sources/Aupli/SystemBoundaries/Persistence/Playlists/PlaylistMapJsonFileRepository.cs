// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaylistMapJsonFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Playlists
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Playlist;
    using Aupli.DomainServices.Shared.Playlist;
    using Newtonsoft.Json;

    /// <summary>
    /// Persists an loads a map of the available playlists.
    /// </summary>
    public class PlaylistMapJsonFileRepository : IPlaylistRepository
    {
        private readonly string filePath;
        private Dictionary<string, PlaylistEntity> playlists = new Dictionary<string, PlaylistEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMapJsonFileRepository"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public PlaylistMapJsonFileRepository(string filePath)
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
            this.playlists =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(await File.ReadAllTextAsync(this.filePath).ConfigureAwait(false))
                    .ToDictionary(pair => pair.Key, pair => new PlaylistEntity(pair.Key, pair.Value));
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        public Task<PlaylistEntity> GetPlaylistAsync(string id)
        {
            if (this.playlists.TryGetValue(id, out var playlist))
            {
                return Task.FromResult(playlist);
            }

            return Task.FromResult(default(PlaylistEntity));
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        public async Task SaveAsync()
        {
            await File.WriteAllTextAsync(this.filePath, JsonConvert.SerializeObject(this.playlists.ToDictionary(x => x.Key, x => x.Value.Name))).ConfigureAwait(false);
        }
    }
}