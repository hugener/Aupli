// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeAdjustmentService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.DomainServices.Interface.Volume
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing a volume service.
    /// </summary>
    public interface IVolumeAdjustmentService
    {
        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        ApplicationServices.Shared.Volume.Volume Volume { get; }

        /// <summary>
        /// Adjusts the specified is increment.
        /// </summary>
        /// <param name="isIncrement">if set to <c>true</c> [is increment].</param>
        /// <returns>The new <see cref="Volume"/>.</returns>
        Task<ApplicationServices.Shared.Volume.Volume> AdjustVolumeAsync(bool isIncrement);
    }
}