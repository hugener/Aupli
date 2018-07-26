// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastPlaylistService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices.Playlist
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Interface.Playlist;
    using Aupli.DomainServices.RequiredInterface.Playlist;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Service for maintaining the last playlist.
    /// </summary>
    public class LastPlaylistService : IInitializable, ILastPlaylistService
    {
        private readonly ILastPlaylistChangeHandler lastPlaylistChangeHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastPlaylistService"/> class.
        /// </summary>
        /// <param name="lastPlaylistChangeHandler">The last playlist repository.</param>
        public LastPlaylistService(ILastPlaylistChangeHandler lastPlaylistChangeHandler)
        {
            this.lastPlaylistChangeHandler = lastPlaylistChangeHandler;
        }

        /// <summary>
        /// Gets the last playlist.
        /// </summary>
        /// <value>
        /// The last playlist.
        /// </value>
        public Shared.Playlist.PlaylistEntity LastPlaylist { get; private set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            this.LastPlaylist = await this.lastPlaylistChangeHandler.GetLastPlaylistAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the last playlist.
        /// </summary>
        /// <param name="playlist">The playlist.</param>
        /// <returns>An async task.</returns>
        public async Task ChangeLastPlaylistAsync(Shared.Playlist.PlaylistEntity playlist)
        {
            if (!Equals(this.LastPlaylist, playlist))
            {
                this.LastPlaylist = playlist;
                await this.lastPlaylistChangeHandler.SetLastPlaylistAsync(playlist);
            }
        }
    }
}