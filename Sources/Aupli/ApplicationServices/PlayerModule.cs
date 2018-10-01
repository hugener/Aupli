// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player;
    using Aupli.ApplicationServices.Player.Api;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.DomainServices.Playlist.Api;
    using Sundew.Base.Initialization;

    /// <summary>
    /// The player module.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class PlayerModule : IInitializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerModule" /> class.
        /// </summary>
        /// <param name="playlistRepository">The playlist repository.</param>
        /// <param name="lastPlaylistService">The last playlist service.</param>
        /// <param name="playbackControls">The playback controls.</param>
        /// <param name="playerServiceReporter">The player service reporter.</param>
        public PlayerModule(
            IPlaylistRepository playlistRepository,
            ILastPlaylistService lastPlaylistService,
            IPlaybackControls playbackControls,
            IPlayerServiceReporter playerServiceReporter)
        {
            this.PlayerService = new PlayerService(
                new PlaylistSearchService(playlistRepository),
                lastPlaylistService,
                playbackControls,
                playerServiceReporter);
        }

        /// <summary>
        /// Gets the player service.
        /// </summary>
        /// <value>
        /// The player service.
        /// </value>
        public IPlayerService PlayerService { get; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            await this.PlayerService.InitializeAsync();
        }
    }
}