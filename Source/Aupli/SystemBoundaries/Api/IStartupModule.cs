// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartupModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Api
{
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;

    /// <summary>
    /// Interface for the startup module.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Bridges.Interaction.IUserInterfaceBridge" />
    public interface IStartupModule : IUserInterfaceBridge
    {
        /// <summary>
        /// Gets the lifecycle configuration.
        /// </summary>
        /// <value>
        /// The lifecycle configuration.
        /// </value>
        ILifecycleConfiguration LifecycleConfiguration { get; }

        /// <summary>Navigates to startup view asynchronous.</summary>
        /// <returns>An async task.</returns>
        Task NavigateToStartupViewAsync();
    }
}