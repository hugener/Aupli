// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.RequiredInterface.Volume
{
    using System.Threading.Tasks;
    using Sundew.Base.Initialization;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Repository for accessing the volume.
    /// </summary>
    public interface IVolumeRepository : IInitializable
    {
        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        Percentage Volume { get; set; }

        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        Task SaveAsync();
    }
}