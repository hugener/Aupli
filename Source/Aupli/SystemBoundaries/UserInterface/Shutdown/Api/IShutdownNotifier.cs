// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShutdownNotifier.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Shutdown.Api
{
    using System;

    /// <summary>
    /// Interface for notifying about shutdown.
    /// </summary>
    public interface IShutdownNotifier
    {
        /// <summary>
        /// Occurs when [shutdown].
        /// </summary>
        event EventHandler ShuttingDown;
    }
}