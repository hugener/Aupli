// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartupModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Api
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Public interface for the startup module.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Bridges.Interaction.IUserInterfaceBridge" />
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    /// <seealso cref="System.IDisposable" />
    public interface IStartupModule : IUserInterfaceBridge, IInitializable, IDisposable
    {
        /// <summary>
        /// Gets the lifecycle configuration.
        /// </summary>
        /// <value>
        /// The lifecycle configuration.
        /// </value>
        ILifecycleConfiguration LifecycleConfiguration { get; }
    }
}