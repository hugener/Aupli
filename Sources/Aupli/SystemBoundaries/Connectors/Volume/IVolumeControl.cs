// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeControl.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Connectors.Volume
{
    using Aupli.ApplicationServices.RequiredInterface.Volume;
    using global::System.Threading.Tasks;
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
        Task SetVolumeAsync(Percentage volume);
    }
}