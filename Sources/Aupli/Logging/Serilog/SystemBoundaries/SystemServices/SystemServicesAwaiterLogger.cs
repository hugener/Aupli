// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemServicesAwaiterLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.SystemServices
{
    using System;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using global::Serilog;
    using global::Serilog.Events;
    using Sundew.Base;

    /// <summary>
    /// Log to Serilog for <see cref="ISystemServicesAwaiterReporter"/>.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.SystemServices.Ari.ISystemServicesAwaiterReporter" />
    public class SystemServicesAwaiterLogger : ISystemServicesAwaiterReporter
    {
        private readonly LogEventLevel logEventLevel;
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiterLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logEventLevel">The log event level.</param>
        public SystemServicesAwaiterLogger(ILogger logger, LogEventLevel logEventLevel = LogEventLevel.Debug)
        {
            this.logger = logger;
            this.logEventLevel = logEventLevel;
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.logger = this.logger.ForContext(source.AsType());
        }

        /// <summary>
        /// Services the is running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public void ServiceIsRunning(string serviceName)
        {
            this.logger.Write(this.logEventLevel, "{ServiceName} is running", serviceName);
        }

        /// <summary>
        /// Services the is not running.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public void ServiceIsNotRunning(string serviceName)
        {
            this.logger.Write(this.logEventLevel, "{ServiceName} is not running", serviceName);
        }

        /// <summary>
        /// Checks the service error.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="exception">The exception.</param>
        public void CheckServiceError(string serviceName, Exception exception)
        {
            this.logger.Write(this.logEventLevel, exception, "Check service {ServiceName} resulted in an error", serviceName);
        }

        /// <summary>
        /// Waits for service timed out.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        public void WaitForServiceTimedOut(string serviceName)
        {
            this.logger.Write(this.logEventLevel, "Check service {ServiceName} timed out", serviceName);
        }
    }
}