// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMusicPlayer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.MusicControl
{
    using System;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Volume.Ari;

    /// <summary>
    /// Interface for a music player.
    /// </summary>
    /// <seealso cref="IVolumeControl" />
    /// <seealso cref="IPlaybackControls" />
    /// <seealso cref="System.IDisposable" />
    public interface IMusicPlayer : IVolumeControl, IPlaybackControls, IAudioOutputStatusUpdater, IDisposable
    {
    }
}