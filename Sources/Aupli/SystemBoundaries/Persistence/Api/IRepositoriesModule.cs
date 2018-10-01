// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoriesModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Api
{
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Playlist.Ari;
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Interface for the repositories modules.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public interface IRepositoriesModule : IInitializable
    {
        /// <summary>
        /// Gets the volume repository.
        /// </summary>
        /// <value>
        /// The volume repository.
        /// </value>
        IVolumeRepository VolumeRepository { get; }

        /// <summary>
        /// Gets the playlist repository.
        /// </summary>
        /// <value>
        /// The playlist repository.
        /// </value>
        IPlaylistRepository PlaylistRepository { get; }

        /// <summary>
        /// Gets the last playlist repository.
        /// </summary>
        /// <value>
        /// The last playlist repository.
        /// </value>
        ILastPlaylistRepository LastPlaylistRepository { get; }
    }
}