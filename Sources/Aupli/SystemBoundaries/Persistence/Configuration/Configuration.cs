// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Configuration
{
    using System;
    using Aupli.SystemBoundaries.Connectors.Lifecycle;
    using Aupli.SystemBoundaries.Connectors.Timeouts;

    /// <summary>
    /// Contains configuration for the application lifecycle.
    /// </summary>
    public class Configuration : ILifecycleConfiguration, ITimeoutConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="lastGreeting">The last greeting.</param>
        /// <param name="idleTimeout">The idle timeout.</param>
        /// <param name="systemTimeout">The system timeout.</param>
        public Configuration(
            string name,
            string lastGreeting,
            TimeSpan idleTimeout,
            TimeSpan systemTimeout)
        {
            this.Name = name;
            this.LastGreeting = lastGreeting;
            this.IdleTimeout = idleTimeout;
            this.SystemTimeout = systemTimeout;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the last greeting.
        /// </summary>
        /// <value>
        /// The last greeting.
        /// </value>
        public string LastGreeting { get; set; }

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