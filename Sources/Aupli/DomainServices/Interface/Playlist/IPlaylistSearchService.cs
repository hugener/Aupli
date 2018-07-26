// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlaylistSearchService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices.Interface.Playlist
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing a playlist search service.
    /// </summary>
    public interface IPlaylistSearchService
    {
        /// <summary>
        /// Gets the playlist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The playlist name.
        /// </returns>
        Task<Shared.Playlist.PlaylistEntity> GetPlaylistAsync(string id);
    }
}