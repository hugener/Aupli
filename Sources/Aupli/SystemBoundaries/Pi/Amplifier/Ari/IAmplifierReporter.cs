// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAmplifierReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Amplifier.Ari
{
    using Sundew.Base.Numeric;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter for <see cref="Max9744Amplifier"/>.
    /// </summary>
    public interface IAmplifierReporter : IReporter
    {
        /// <summary>
        /// Changes the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        void ChangeMute(bool isMuted);

        /// <summary>
        /// Changes the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        void ChangeVolume(Percentage volume);
    }
}