// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerStatusUpdater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.RequiredInterface.Player
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing player info.
    /// </summary>
    public interface IPlayerStatusUpdater
    {
        /// <summary>
        /// Occurs when status has changed.
        /// </summary>
        event EventHandler<StatusEventArgs> StatusChanged;

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        PlayerStatus Status { get; }

        /// <summary>
        /// Updates the status asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        Task UpdateStatusAsync();
    }
}