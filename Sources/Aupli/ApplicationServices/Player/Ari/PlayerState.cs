// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerState.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Aupli.ApplicationServices.Player.Ari
{
    /// <summary>
    /// Defines the player state.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The player is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The player is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The player is stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The player state is unknown.
        /// </summary>
        Unknown,
    }
}