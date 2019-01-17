// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlsModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Controls
{
    using System;
    using Sundew.Base.Threading;

    /// <summary>
    /// Required interface with various controls for user interface module.
    /// </summary>
    public interface IControlsModuleFactory : IDisposable
    {
        /// <summary>
        /// Gets the system control.
        /// </summary>
        /// <value>
        /// The system control.
        /// </value>
        IAsyncLazy<IControlsModule> ControlsModule { get; }
    }
}