// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMusicPlayerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Mpc
{
    using System;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for logging music player activity.
    /// </summary>
    public interface IMusicPlayerReporter : IReporter
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
    }
}