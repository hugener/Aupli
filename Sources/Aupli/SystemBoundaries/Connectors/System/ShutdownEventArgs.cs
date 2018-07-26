// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Connectors.System
{
    using global::System;

    /// <summary>
    /// Event args with shutdown cancellation support.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public abstract class ShutdownEventArgs : EventArgs
    {
        /// <summary>
        /// Cancels the shutdown.
        /// </summary>
        public abstract void CancelShutdown();
    }
}