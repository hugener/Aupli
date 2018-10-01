// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlaylistRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Player.Ari
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Playlist.Shared;
    using Sundew.Base.Initialization;

    /// <summary>
    /// The playlist repository.
    /// </summary>
    public interface IPlaylistRepository : IInitializable
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The entity.
        /// </returns>
        Task<PlaylistEntity> GetPlaylistAsync(string id);

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        Task SaveAsync();
    }
}