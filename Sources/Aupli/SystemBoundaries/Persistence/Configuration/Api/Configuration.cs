// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Configuration.Api
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Timeouts;

    /// <summary>
    /// Contains configuration for the application lifecycle.
    /// </summary>
    public class Configuration : ITimeoutConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="idleTimeout">The idle timeout.</param>
        /// <param name="systemTimeout">The system timeout.</param>
        public Configuration(
            TimeSpan idleTimeout,
            TimeSpan systemTimeout)
        {
            this.IdleTimeout = idleTimeout;
            this.SystemTimeout = systemTimeout;
        }

        /// <summary>
        /// Gets the idle timeout.
        /// </summary>
        /// <value>
        /// The idle timeout.
        /// </value>
        public TimeSpan IdleTimeout { get; }

        /// <summary>
        /// Gets the system timeout.
        /// </summary>
        /// <value>
        /// The system timeout.
        /// </value>
        public TimeSpan SystemTimeout { get; }
    }
}