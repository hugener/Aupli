﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeControl.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Ari
{
    using System.Threading.Tasks;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Interface for implementing a volume control.
    /// </summary>
    public interface IVolumeControl : IVolumeStatusUpdater
    {
        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <returns>An async task.</returns>
        Task<bool> SetVolumeAsync(Percentage volume);

        /// <summary>
        /// Sets the state of the mute.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        Task MuteAsync();
    }
}