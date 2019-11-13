// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemServicesModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using Aupli.SystemBoundaries.SystemServices.Unix;
    using Sundew.Base.Initialization;

    /// <summary>Module for starting system services.</summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class SystemServicesModule : IInitializable
    {
        private readonly ISystemServicesAwaiterReporter systemServicesAwaiterReporter;
        private readonly IWifiConnecterReporter wifiConnecterReporter;

        /// <summary>Initializes a new instance of the <see cref="SystemServicesModule"/> class.</summary>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        /// <param name="wifiConnecterReporter">The wifi connecter reporter.</param>
        public SystemServicesModule(
            ISystemServicesAwaiterReporter systemServicesAwaiterReporter,
            IWifiConnecterReporter wifiConnecterReporter)
        {
            this.systemServicesAwaiterReporter = systemServicesAwaiterReporter;
            this.wifiConnecterReporter = wifiConnecterReporter;
        }

        /// <summary>Initializes the asynchronous.</summary>
        /// <returns>An async task.</returns>
        public async ValueTask InitializeAsync()
        {
            await Task.Run(async () =>
            {
                var systemServicesAwaiter = this.CreateServicesAwaiter();
                await systemServicesAwaiter.WaitForServicesAsync(new[] { "mpd" }, Timeout.InfiniteTimeSpan);
            });

            var task = this.CreateWifiConnecter().TryConnectAsync(x => x.StartsWith("A"), CancellationToken.None);
        }

        /// <summary>
        /// Creates the services awaiter.
        /// </summary>
        /// <returns>A <see cref="UnixSystemServiceStateChecker"/>.</returns>
        protected virtual ISystemServicesAwaiter CreateServicesAwaiter()
        {
            return new SystemServicesAwaiter(this.systemServicesAwaiterReporter);
        }

        /// <summary>Creates the wifi connecter.</summary>
        /// <returns>A <see cref="WifiConnecter"/>.</returns>
        protected virtual IWifiConnecter CreateWifiConnecter()
        {
            return new WifiConnecter(this.wifiConnecterReporter);
        }
    }
}