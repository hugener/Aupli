// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerServiceReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Player.Ari
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing a reporter for <see cref="PlayerService"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IPlayerServiceReporter : IReporter
    {
        /// <summary>
        /// Founds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        void FoundPlaylist(string playlistName);

        /// <summary>
        /// Dids the not find playlist for identifier.
        /// </summary>
        /// <param name="playlistId">The playlist identifier.</param>
        void DidNotFindPlaylistForId(string playlistId);
    }
}