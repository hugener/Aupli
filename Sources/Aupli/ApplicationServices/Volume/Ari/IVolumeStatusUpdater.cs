// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeStatusUpdater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Ari
{
    using System;

    /// <summary>
    /// Interface for getting volume info.
    /// </summary>
    public interface IVolumeStatusUpdater
    {
        /// <summary>
        /// Occurs when [volume changed].
        /// </summary>
        event EventHandler<VolumeChangedEventArgs> VolumeChanged;
    }
}