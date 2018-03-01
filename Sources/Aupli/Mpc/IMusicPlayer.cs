// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMusicPlayer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Mpc
{
    using System;

    /// <summary>
    /// Interface for implementing a music player.
    /// </summary>
    /// <seealso cref="IVolumeControl" />
    /// <seealso cref="System.IDisposable" />
    public interface IMusicPlayer : IPlayerInfo, IPlaybackControls, IVolumeControl, IDisposable
    {
    }
}