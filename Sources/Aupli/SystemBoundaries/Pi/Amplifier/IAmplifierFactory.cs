// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAmplifierFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Amplifier
{
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Disposal;

    /// <summary>
    /// Interface for implementing an amplifer factory.
    /// </summary>
    /// <seealso cref="IAssociatedDisposer{IAmplifier}" />
    public interface IAmplifierFactory : IAssociatedDisposer<IAmplifier>
    {
        /// <summary>
        /// Creates the specified pin26 feature.
        /// </summary>
        /// <param name="amplifierReporter">The amplifier reporter.</param>
        /// <returns>
        /// The volume controls.
        /// </returns>
        IAmplifier Create(IAmplifierReporter amplifierReporter);
    }
}