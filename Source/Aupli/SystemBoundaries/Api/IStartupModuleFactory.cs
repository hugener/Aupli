// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStartupModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Api
{
    using System;
    using Sundew.Base.Threading;

    /// <summary>
    /// Public interface for the startup module.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Bridges.Interaction.IUserInterfaceBridge" />
    /// <seealso cref="System.IDisposable" />
    public interface IStartupModuleFactory : IDisposable
    {
        /// <summary>
        /// Gets the startup module data.
        /// </summary>
        /// <value>
        /// The startup module data.
        /// </value>
        IAsyncLazy<IStartupModule> StartupModule { get; }
    }
}