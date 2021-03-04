// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAudioOutputStatusUpdater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Ari
{
    using System;

    /// <summary>
    /// Interface for implementing player info.
    /// </summary>
    public interface IAudioOutputStatusUpdater
    {
        /// <summary>
        /// Occurs when status has changed.
        /// </summary>
        event EventHandler AudioOutputStatusChanged;

        /// <summary>
        /// Gets a value indicating whether this instance is outputting audio.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is outputting audio; otherwise, <c>false</c>.
        /// </value>
        bool IsAudioOutputActive { get; }
    }
}