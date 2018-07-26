// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILifecycleConfiguration.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Connectors.Lifecycle
{
    /// <summary>
    /// Interface for getting the start up configuration.
    /// </summary>
    public interface ILifecycleConfiguration
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the last greeting.
        /// </summary>
        /// <value>
        /// The last greeting.
        /// </value>
        string LastGreeting { get; set; }
    }
}