// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemServicesAwaiter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using Aupli.SystemBoundaries.SystemServices.Unix;
    using Aupli.SystemBoundaries.SystemServices.Windows;

    /// <summary>
    /// Waits until all specified services are running.
    /// </summary>
    /// <seealso cref="ISystemServicesAwaiter" />
    public class SystemServicesAwaiter : ISystemServicesAwaiter
    {
        private readonly TimeSpan delay;
        private readonly ISystemServiceStateChecker systemServiceStateChecker;
        private readonly ISystemServicesAwaiterReporter? systemServicesAwaiterReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter" /> class.
        /// </summary>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        public SystemServicesAwaiter(ISystemServicesAwaiterReporter? systemServicesAwaiterReporter = null)
            : this(TimeSpan.FromSeconds(1), systemServicesAwaiterReporter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter" /> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        public SystemServicesAwaiter(TimeSpan delay, ISystemServicesAwaiterReporter? systemServicesAwaiterReporter = null)
        : this(delay, GetSystemServiceChecker(), systemServicesAwaiterReporter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter" /> class.
        /// </summary>
        /// <param name="delay">The delay interval.</param>
        /// <param name="systemServiceStateChecker">The system service state checker.</param>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        public SystemServicesAwaiter(TimeSpan delay, ISystemServiceStateChecker systemServiceStateChecker, ISystemServicesAwaiterReporter? systemServicesAwaiterReporter = null)
        {
            this.delay = delay;
            this.systemServiceStateChecker = systemServiceStateChecker;
            this.systemServicesAwaiterReporter = systemServicesAwaiterReporter;
            this.systemServicesAwaiterReporter?.SetSource(this);
        }

        /// <summary>
        /// Waits for services asynchronous.
        /// </summary>
        /// <param name="servicesNames">The services names.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An async task indicating whether all services has been started.</returns>
        public async Task<bool> WaitForServicesAsync(IEnumerable<string> servicesNames, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            timeoutCancellationTokenSource.CancelAfter(timeout);
            var timeoutCancellationToken = timeoutCancellationTokenSource.Token;
            var result = await Task.WhenAll(servicesNames.Select(serviceName =>
            {
                return Task.Run(
                    () => this.CheckServiceIsStarted(serviceName, timeoutCancellationToken),
                    timeoutCancellationToken);
            })).ConfigureAwait(false);
            return result.All(x => x);
        }

        private static ISystemServiceStateChecker GetSystemServiceChecker()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return new UnixSystemServiceStateChecker();
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    return new WindowsSystemServiceStateChecker();
                default:
                    throw new NotSupportedException($"{Environment.OSVersion.Platform} is not supported.");
            }
        }

        /// <summary>
        /// Checks the service is started.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An async task.</returns>
        private async Task<bool> CheckServiceIsStarted(string serviceName, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (this.systemServiceStateChecker.IsServiceRunning(serviceName))
                    {
                        this.systemServicesAwaiterReporter?.ServiceIsRunning(serviceName);
                        return true;
                    }

                    this.systemServicesAwaiterReporter?.ServiceIsNotRunning(serviceName);
                    await Task.Delay(this.delay, cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception e)
                {
                    this.systemServicesAwaiterReporter?.CheckServiceError(serviceName, e);
                }
            }

            this.systemServicesAwaiterReporter?.WaitForServiceTimedOut(serviceName);
            return false;
        }
    }
}