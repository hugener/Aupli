// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoriesModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Playlist.Ari;
    using Aupli.ApplicationServices.Volume.Ari;
    using Aupli.SystemBoundaries.Persistence.Api;
    using Aupli.SystemBoundaries.Persistence.Configuration;
    using Aupli.SystemBoundaries.Persistence.Configuration.Api;
    using Aupli.SystemBoundaries.Persistence.Playlists;
    using Aupli.SystemBoundaries.Persistence.Volume;

    /// <summary>
    /// Contains the repositories.
    /// </summary>
    public class RepositoriesModule : IRepositoriesModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoriesModule" /> class.
        /// </summary>
        /// <param name="volumeRepositoryPath">The volume repository path.</param>
        /// <param name="playlistRepositoryPath">The playlist repository path.</param>
        /// <param name="lastPlaylistRepositoryPath">The last playlist repository path.</param>
        /// <param name="configurationPath">The configuration path.</param>
        public RepositoriesModule(
            string volumeRepositoryPath,
            string playlistRepositoryPath,
            string lastPlaylistRepositoryPath,
            string configurationPath)
        {
            this.VolumeRepository = new VolumeJsonFileRepository(volumeRepositoryPath);
            this.PlaylistRepository = new PlaylistMapJsonFileRepository(playlistRepositoryPath);
            this.LastPlaylistRepository = new LastPlaylistJsonFileRepository(lastPlaylistRepositoryPath);
            this.ConfigurationRepository = new ConfigurationJsonFileRepository(configurationPath);
        }

        /// <summary>
        /// Gets the configuration repository.
        /// </summary>
        /// <value>
        /// The configuration repository.
        /// </value>
        public IConfigurationRepository ConfigurationRepository { get; }

        /// <summary>
        /// Gets the volume repository.
        /// </summary>
        /// <value>
        /// The volume repository.
        /// </value>
        public IVolumeRepository VolumeRepository { get; }

        /// <summary>
        /// Gets the playlist repository.
        /// </summary>
        /// <value>
        /// The playlist repository.
        /// </value>
        public IPlaylistRepository PlaylistRepository { get; }

        /// <summary>
        /// Gets the last playlist repository.
        /// </summary>
        /// <value>
        /// The last playlist repository.
        /// </value>
        public ILastPlaylistRepository LastPlaylistRepository { get; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            await this.VolumeRepository.InitializeAsync();
        }
    }
}