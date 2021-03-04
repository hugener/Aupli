// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWifiConnecterReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices.Ari
{
    using System.Collections.Generic;
    using Sundew.Base.Reporting;

    /// <summary>Interface for implementing a wifi connecter reporter.</summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IWifiConnecterReporter : IReporter
    {
        /// <summary>Matcheds the profiles.</summary>
        /// <param name="wifiProfiles">The wifi profiles.</param>
        void MatchedProfiles(IReadOnlyList<string> wifiProfiles);

        /// <summary>Connecteds to wifi.</summary>
        /// <param name="wifiProfile">The wifi profile.</param>
        void ConnectedToWifi(string wifiProfile);

        /// <summary>Coulds the not connect to wifi.</summary>
        /// <param name="wifiProfile">The wifi profile.</param>
        void CouldNotConnectToWifi(string wifiProfile);

        /// <summary>Connections the cancelled.</summary>
        void ConnectionCancelled();
    }
}