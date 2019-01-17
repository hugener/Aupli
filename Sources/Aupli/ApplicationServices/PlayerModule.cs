// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player;
    using Aupli.ApplicationServices.Player.Api;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.DomainServices.Playlist.Api;
    using Aupli.SystemBoundaries.SystemServices;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using Aupli.SystemBoundaries.SystemServices.Unix;
    using Sundew.Base.Initialization;

    /// <summary>
    /// The player module.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class PlayerModule : IInitializable
    {
        private readonly ISystemServicesAwaiterReporter systemServicesAwaiterReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerModule" /> class.
        /// </summary>
        /// <param name="playlistRepository">The playlist repository.</param>
        /// <param name="lastPlaylistService">The last playlist service.</param>
        /// <param name="playbackControls">The playback controls.</param>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        /// <param name="playerServiceReporter">The player service reporter.</param>
        public PlayerModule(
            IPlaylistRepository playlistRepository,
            ILastPlaylistService lastPlaylistService,
            IPlaybackControls playbackControls,
            ISystemServicesAwaiterReporter systemServicesAwaiterReporter,
            IPlayerServiceReporter playerServiceReporter)
        {
            this.systemServicesAwaiterReporter = systemServicesAwaiterReporter;
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
            await Task.Run(async () =>
            {
                var systemServicesAwaiter = this.CreateServicesAwaiter();
                await systemServicesAwaiter.WaitForServicesAsync(new[] { "mpd" }, Timeout.InfiniteTimeSpan);
                await this.PlayerService.InitializeAsync();
            });
        }

        /// <summary>
        /// Creates the services awaiter.
        /// </summary>
        /// <returns>A <see cref="UnixSystemServiceStateChecker"/>.</returns>
        protected virtual ISystemServicesAwaiter CreateServicesAwaiter()
        {
            return new SystemServicesAwaiter(this.systemServicesAwaiterReporter);
        }
    }
}