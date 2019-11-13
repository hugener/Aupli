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
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.DomainServices.Playlist.Shared;
    using Newtonsoft.Json;
    using Sundew.Base.Threading;

    /// <summary>
    /// Persists an loads a map of the available playlists.
    /// </summary>
    public class PlaylistMapJsonFileRepository : IPlaylistRepository
    {
        private readonly string filePath;

        private readonly AsyncLazy<Dictionary<string, PlaylistEntity>> playlists;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMapJsonFileRepository"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public PlaylistMapJsonFileRepository(string filePath)
        {
            this.filePath = filePath;
            this.playlists =
                new AsyncLazy<Dictionary<string, PlaylistEntity>>(
                    async () =>
                    {
                        var playlistJson = await File.ReadAllTextAsync(this.filePath).ConfigureAwait(false);
                        if (string.IsNullOrEmpty(playlistJson))
                        {
                            return new Dictionary<string, PlaylistEntity>();
                        }

                        return JsonConvert.DeserializeObject<Dictionary<string, string>>(playlistJson)
                            .ToDictionary(pair => pair.Key, pair => new PlaylistEntity(pair.Key, pair.Value));
                    },
                    true);
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        public async Task<PlaylistEntity?> GetPlaylistAsync(string id)
        {
            var playlists = await this.playlists;
            if (playlists.TryGetValue(id, out var playlist))
            {
                return playlist;
            }

            return default;
        }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        public async Task SaveAsync()
        {
            var playlists = await this.playlists;
            if (playlists != null)
            {
                await File.WriteAllTextAsync(this.filePath, JsonConvert.SerializeObject(playlists.ToDictionary(x => x.Key, x => x.Value.Name))).ConfigureAwait(false);
            }
        }
    }
}