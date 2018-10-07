// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeoutConfiguration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Timeouts
{
    using System;

    /// <summary>
    /// Interface for the timeout configuration.
    /// </summary>
    public interface ITimeoutConfiguration
    {
        /// <summary>
        /// Gets the idle timeout.
        /// </summary>
        /// <value>
        /// The idle timeout.
        /// </value>
        TimeSpan IdleTimeout { get; }

        /// <summary>
        /// Gets the system timeout.
        /// </summary>
        /// <value>
        /// The system timeout.
        /// </value>
        TimeSpan SystemTimeout { get; }
    }
}