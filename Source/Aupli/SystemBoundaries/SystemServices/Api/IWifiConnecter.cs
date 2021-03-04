// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWifiConnecter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Api
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Interface for trying to connect to wifi.</summary>
    public interface IWifiConnecter
    {
        /// <summary>Activates the wifi asynchronous.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An async task.</returns>
        Task TryConnectAsync(CancellationToken cancellationToken);

        /// <summary>Activates the wifi asynchronous.</summary>
        /// <param name="profileFilter">The filter to select, which profiles to try to connect to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An async task.</returns>
        Task TryConnectAsync(Func<string, bool> profileFilter, CancellationToken cancellationToken);
    }
}