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

    /// <summary>
    /// Waits until all specified services are running.
    /// </summary>
    /// <seealso cref="ISystemServicesAwaiter" />
    public class SystemServicesAwaiter : ISystemServicesAwaiter
    {
        private readonly TimeSpan delay;
        private readonly ISystemServiceStateChecker systemServiceStateChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter"/> class.
        /// </summary>
        public SystemServicesAwaiter()
            : this(TimeSpan.FromSeconds(1))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter" /> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        public SystemServicesAwaiter(TimeSpan delay)
        : this(delay, GetSystemServiceChecker())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemServicesAwaiter" /> class.
        /// </summary>
        /// <param name="delay">The delay interval.</param>
        /// <param name="systemServiceStateChecker">The system service state checker.</param>
        public SystemServicesAwaiter(TimeSpan delay, ISystemServiceStateChecker systemServiceStateChecker)
        {
            this.delay = delay;
            this.systemServiceStateChecker = systemServiceStateChecker;
        }

        /// <summary>
        /// Waits for services asynchronous.
        /// </summary>
        /// <param name="servicesNames">The services names.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>An async task indicating whether all services has been started.</returns>
        public async Task<bool> WaitForServicesAsync(IEnumerable<string> servicesNames, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                timeoutCancellationTokenSource.CancelAfter(timeout);
                var timeoutCancellationToken = timeoutCancellationTokenSource.Token;
                var result = await Task.WhenAll(servicesNames.Select(serviceName =>
                {
                    return Task.Run(
                        async () => await this.CheckServiceIsStarted(serviceName, timeoutCancellationToken),
                        timeoutCancellationToken);
                }));
                return result.All(x => x);
            }
        }

        private static ISystemServiceStateChecker GetSystemServiceChecker()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return new UnixSystemServiceStateChecker();
                default:
                    throw new NotSupportedException("Currently only Unix is supported.");
            }
        }

        private async Task<bool> CheckServiceIsStarted(string serviceName, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (this.systemServiceStateChecker.IsServiceRunning(serviceName))
                    {
                        return true;
                    }

                    await Task.Delay(this.delay, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
            }

            return false;
        }
    }
}