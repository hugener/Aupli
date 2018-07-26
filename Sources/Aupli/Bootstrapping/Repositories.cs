// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repositories.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Bootstrapping
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Playlist;
    using Aupli.ApplicationServices.RequiredInterface.Volume;
    using Aupli.DomainServices.RequiredInterface.Playlist;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Contains the repositories.
    /// </summary>
    public class Repositories : IInitializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repositories"/> class.
        /// </summary>
        /// <param name="volumeRepository">The volume repository.</param>
        /// <param name="playlistRepository">The playlist repository.</param>
        /// <param name="lastPlaylistRepository">The last playlist repository.</param>
        public Repositories(
            IVolumeRepository volumeRepository,
            IPlaylistRepository playlistRepository,
            ILastPlaylistRepository lastPlaylistRepository)
        {
            this.VolumeRepository = volumeRepository;
            this.PlaylistRepository = playlistRepository;
            this.LastPlaylistRepository = lastPlaylistRepository;
        }

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
            await this.PlaylistRepository.InitializeAsync();
            await this.LastPlaylistRepository.InitializeAsync();
            await this.VolumeRepository.InitializeAsync();
        }
    }
}