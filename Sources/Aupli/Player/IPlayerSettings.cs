// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerSettings.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Aupli.Player
{
    /// <summary>
    /// Represents settings for the player.
    /// </summary>
    public interface IPlayerSettings
    {
        /// <summary>
        /// Gets or sets the last playlist.
        /// </summary>
        /// <value>
        /// The last playlist.
        /// </value>
        string LastPlaylist { get; set; }
    }
}