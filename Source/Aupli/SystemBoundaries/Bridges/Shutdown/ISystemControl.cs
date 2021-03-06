﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemControl.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Shutdown
{
    using System;

    /// <summary>
    /// Interface for controlling the system.
    /// </summary>
    public interface ISystemControl
    {
        /// <summary>
        /// Occurs when [shutting down].
        /// </summary>
        event EventHandler<ShutdownEventArgs> ShuttingDown;

        /// <summary>
        /// Shutdowns this instance.
        /// </summary>
        void Shutdown();
    }
}