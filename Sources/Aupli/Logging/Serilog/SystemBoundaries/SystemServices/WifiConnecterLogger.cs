// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WifiConnecterLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.SystemServices
{
    using System.Collections.Generic;
    using System.Reflection;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>Implementation of <see cref="IWifiConnecterReporter"/> for logging to Serilog.</summary>
    /// <seealso cref="Aupli.SystemBoundaries.SystemServices.Ari.IWifiConnecterReporter" />
    public class WifiConnecterLogger : IWifiConnecterReporter
    {
        private ILogger logger;

        /// <summary>Initializes a new instance of the <see cref="WifiConnecterLogger"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public WifiConnecterLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>Sets the source.</summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.logger = this.logger.ForContext(source.AsType());
        }

        /// <summary>Connecteds to wifi.</summary>
        /// <param name="wifiProfile">The wifi profile.</param>
        public void ConnectedToWifi(string wifiProfile)
        {
            this.logger.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{WifiProfile}}", wifiProfile);
        }

        /// <summary>Connections the cancelled.</summary>
        public void ConnectionCancelled()
        {
            this.logger.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>Coulds the not connect to wifi.</summary>
        /// <param name="wifiProfile">The wifi profile.</param>
        public void CouldNotConnectToWifi(string wifiProfile)
        {
            this.logger.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{WifiProfile}}", wifiProfile);
        }

        /// <summary>Matcheds the profiles.</summary>
        /// <param name="wifiProfiles">The wifi profiles.</param>
        public void MatchedProfiles(IReadOnlyList<string> wifiProfiles)
        {
            this.logger.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{WifiProfiles}}", wifiProfiles);
        }
    }
}