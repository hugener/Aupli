// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INetworkDeviceInfoProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Shared.System
{
    using global::System.Collections.Generic;

    /// <summary>
    /// Interface for implementing a provider for <see cref="NetworkDeviceInfo"/>.
    /// </summary>
    public interface INetworkDeviceInfoProvider
    {
        /// <summary>
        /// Tries the get network devices.
        /// </summary>
        /// <returns>The network devices.</returns>
        IEnumerable<NetworkDeviceInfo> GetNetworkDevices();
    }
}