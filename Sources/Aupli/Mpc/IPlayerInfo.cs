// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerInfo.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Mpc
{
    using System;
    using System.Threading.Tasks;
    using Aupli.Player;

    /// <summary>
    /// Interface for implementing player info.
    /// </summary>
    public interface IPlayerInfo
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