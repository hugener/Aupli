// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILifecycleConfiguration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Lifecycle
{
    /// <summary>
    /// Interface for getting the start up configuration.
    /// </summary>
    public interface ILifecycleConfiguration
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the pin 26 feature.
        /// </summary>
        /// <value>
        /// The pin 26 feature.
        /// </value>
        Pin26Feature Pin26Feature { get; }
    }
}