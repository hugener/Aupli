// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemServiceStateChecker.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Ari
{
    /// <summary>
    /// Check whether a system service is running.
    /// </summary>
    public interface ISystemServiceStateChecker
    {
        /// <summary>
        /// Checks if the specified service is running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        ///   <c>true</c> if [is service running] [the specified service name]; otherwise, <c>false</c>.
        /// </returns>
        bool IsServiceRunning(string serviceName);
    }
}