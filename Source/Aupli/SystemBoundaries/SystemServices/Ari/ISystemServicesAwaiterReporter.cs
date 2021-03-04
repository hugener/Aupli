// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemServicesAwaiterReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Ari
{
    using System;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for reporting on <see cref="SystemServicesAwaiter"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface ISystemServicesAwaiterReporter : IReporter
    {
        /// <summary>
        /// Services the is running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        void ServiceIsRunning(string serviceName);

        /// <summary>
        /// Services the is not running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        void ServiceIsNotRunning(string serviceName);

        /// <summary>
        /// Checks the service error.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="exception">The exception.</param>
        void CheckServiceError(string serviceName, Exception exception);

        /// <summary>
        /// Waits for service timed out.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        void WaitForServiceTimedOut(string serviceName);
    }
}