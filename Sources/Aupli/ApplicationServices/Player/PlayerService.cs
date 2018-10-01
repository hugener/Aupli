// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Player
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Interface.Player;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Aupli.DomainServices.Interface.Playlist;

    /// <summary>
    /// The player service.
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IPlaylistSearchService playlistSearchService;
        private readonly ILastPlaylistService lastPlaylistService;
        private readonly IPlaybackControls playbackControls;
        private readonly IPlayerServiceReporter playerServiceReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService" /> class.
        /// </summary>
        /// <param name="playlistSearchService">The playlist map service.</param>
        /// <param name="lastPlaylistService">The last playlist service.</param>
        /// <param name="playbackControls">The playback controls.</param>
        /// <param name="playerServiceReporter">The player service reporter.</param>
        public PlayerService(
            IPlaylistSearchService playlistSearchService,
            ILastPlaylistService lastPlaylistService,
            IPlaybackControls playbackControls,
            IPlayerServiceReporter playerServiceReporter)
        {
            this.playlistSearchService = playlistSearchService;
            this.lastPlaylistService = lastPlaylistService;
            this.playbackControls = playbackControls;
            this.playerServiceReporter = playerServiceReporter;
            this.playerServiceReporter?.SetSource(this);
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            await this.playbackControls.UpdateAsync();
            if (this.playbackControls.Status.State != PlayerState.Playing && this.lastPlaylistService.LastPlaylist != null)
            {
                await this.playbackControls.PlayPlaylistAsync(this.lastPlaylistService.LastPlaylist.Name).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Starts the playlist asynchronous.
        /// </summary>
        /// <param name="playlistId">The playlist identifier.</param>
        /// <returns>An async task.</returns>
        public async Task StartPlaylistAsync(string playlistId)
        {
            var playlist = await this.playlistSearchService.GetPlaylistAsync(playlistId).ConfigureAwait(false);
            if (playlist != null)
            {
                this.playerServiceReporter?.FoundPlaylist(playlist.Name);
                if (this.playbackControls.Status.PlaylistName != playlist.Name ||
                    this.playbackControls.Status.Track > 0)
                {
                    await this.playbackControls.PlayPlaylistAsync(playlist.Name).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(playlist.Name))
                    {
                        await this.lastPlaylistService.ChangeLastPlaylistAsync(playlist).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                this.playerServiceReporter?.DidNotFindPlaylistForId(playlistId);
            }
        }
    }
}