// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using Aupli.Numeric;

    /// <summary>
    /// Interface for getting volume info.
    /// </summary>
    public interface IVolumeInfo
    {
        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        Percentage Volume { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        bool IsMuted { get; }
    }
}