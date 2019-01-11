// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMusicPlayerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.MusicControl.Ari
{
    using System;
    using MpcNET;

    /// <summary>
    /// Interface for logging music player activity.
    /// </summary>
    public interface IMusicPlayerReporter : IMpcConnectionReporter
    {
        /// <summary>
        /// Enters the status refresh.
        /// </summary>
        /// <returns>A disposable to exit status refresh.</returns>
        IDisposable EnterStatusRefresh();

        /// <summary>
        /// Startings the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        void StartingPlaylist(string playlistName);

        /// <summary>
        /// Called when [status exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        void OnStatusException(Exception exception);
    }
}