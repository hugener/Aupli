// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeSettings.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Aupli.Volume
{
    /// <summary>
    /// Interface for implementing volume settings.
    /// </summary>
    public interface IVolumeSettings
    {
        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        double Volume { get; set; }
    }
}