// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemServicesAwaiter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Bridge interface for making sure system services has been started.
    /// </summary>
    public interface ISystemServicesAwaiter
    {
        /// <summary>
        /// Waits for services asynchronous.
        /// </summary>
        /// <param name="servicesNames">The services names.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>
        /// An async task.
        /// </returns>
        Task<bool> WaitForServicesAsync(IEnumerable<string> servicesNames, TimeSpan timeout);
    }
}