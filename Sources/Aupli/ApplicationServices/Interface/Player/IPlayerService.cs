// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Interface.Player
{
    using System.Threading.Tasks;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Interface for the player service.
    /// </summary>
    public interface IPlayerService : IInitializable
    {
        /// <summary>
        /// Starts the playlist asynchronous.
        /// </summary>
        /// <param name="playlistId">The playlist identifier.</param>
        /// <returns>An async task.</returns>
        Task StartPlaylistAsync(string playlistId);
    }
}