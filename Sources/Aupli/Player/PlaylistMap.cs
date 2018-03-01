// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaylistMap.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Player
{
    using System.Collections.Generic;

    /// <summary>
    /// Maps a string to a playlist name.
    /// </summary>
    public class PlaylistMap
    {
        private readonly IDictionary<string, string> playlists;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistMap"/> class.
        /// </summary>
        /// <param name="playlists">The playlists.</param>
        internal PlaylistMap(IDictionary<string, string> playlists)
        {
            this.playlists = playlists;
        }

        /// <summary>
        /// Gets the playlist.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The playlist name.</returns>
        public string GetPlaylist(string tag)
        {
            this.playlists.TryGetValue(tag, out var playList);
            return playList;
        }

        /// <summary>
        /// Adds the playlist.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="playlist">The playlist.</param>
        public void AddPlaylist(string tag, string playlist)
        {
            this.playlists.Add(tag, playlist);
        }
    }
}
