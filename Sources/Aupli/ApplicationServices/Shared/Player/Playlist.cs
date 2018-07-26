// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Playlist.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Aupli.ApplicationServices.Shared.Player
{
    /// <summary>
    /// Represents a playlist with a name.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Playlist(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }
    }
}