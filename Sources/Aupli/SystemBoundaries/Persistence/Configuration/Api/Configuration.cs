// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Configuration.Api
{
    using System;
    using Aupli.SystemBoundaries.Shared.Lifecycle;
    using Aupli.SystemBoundaries.Shared.Timeouts;

    /// <summary>
    /// Contains configuration for the application lifecycle.
    /// </summary>
    public class Configuration : ILifecycleConfiguration, ITimeoutConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="idleTimeout">The idle timeout.</param>
        /// <param name="systemTimeout">The system timeout.</param>
        /// <param name="pin26Feature">The pin26 feature.</param>
        public Configuration(
            string name,
            TimeSpan idleTimeout,
            TimeSpan systemTimeout,
            Pin26Feature pin26Feature)
        {
            this.Name = name;
            this.IdleTimeout = idleTimeout;
            this.SystemTimeout = systemTimeout;
            this.Pin26Feature = pin26Feature;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the pin 26 feature.
        /// </summary>
        /// <value>
        /// The pin 26 feature.
        /// </value>
        public Pin26Feature Pin26Feature { get; }

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