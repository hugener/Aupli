// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeServiceReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Ari
{
    using Sundew.Base.Numeric;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter interface for the <see cref="VolumeService"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IVolumeServiceReporter : IReporter
    {
        /// <summary>
        /// Changes the volume.
        /// </summary>
        /// <param name="newVolumePercentage">The new volume percentage.</param>
        void ChangeVolume(Percentage newVolumePercentage);

        /// <summary>
        /// Changes the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [new is muted].</param>
        void ChangeMute(bool isMuted);
    }
}