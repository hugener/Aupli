// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaylistModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Playlist;
    using Aupli.DomainServices.Playlist.Api;
    using Aupli.DomainServices.Playlist.Ari;
    using Sundew.Base.Initialization;

    /// <summary>
    /// The playlist module.
    /// </summary>
    public class PlaylistModule : IInitializable
    {
        private readonly LastPlaylistService lastPlaylistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistModule"/> class.
        /// </summary>
        /// <param name="lastPlaylistChangeHandler">The last playlist change handler.</param>
        public PlaylistModule(ILastPlaylistChangeHandler lastPlaylistChangeHandler)
        {
            this.lastPlaylistService = new LastPlaylistService(lastPlaylistChangeHandler);
        }

        /// <summary>
        /// Gets the last playlist service.
        /// </summary>
        /// <value>
        /// The last playlist service.
        /// </value>
        public ILastPlaylistService LastPlaylistService => this.lastPlaylistService;

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public ValueTask InitializeAsync()
        {
            return this.lastPlaylistService.InitializeAsync();
        }
    }
}