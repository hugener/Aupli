// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WifiConnecter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.SystemServices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using Sundew.Base.Reporting;

    /// <summary>Implement of wifi enabler for Linux (netctl).</summary>
    public class WifiConnecter : IWifiConnecter
    {
        private const string WifiNameGroupName = "WifiName";
        private const string StartCommandText = "switch-to";
        private static readonly Regex WifiListRegex = new Regex($@"^. (?<{WifiNameGroupName}>[\w-_,]+)");
        private readonly IWifiConnecterReporter? wifiConnecterReporter;

        /// <summary>Initializes a new instance of the <see cref="WifiConnecter"/> class.</summary>
        /// <param name="wifiConnecterReporter">The wifi connecter reporter.</param>
        public WifiConnecter(IWifiConnecterReporter? wifiConnecterReporter = null)
        {
            this.wifiConnecterReporter = wifiConnecterReporter;
            this.wifiConnecterReporter?.SetSource(this);
        }

        /// <summary>Activates the wifi asynchronous.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An async task.</returns>
        public Task TryConnectAsync(CancellationToken cancellationToken)
        {
            return this.TryConnectAsync(null, cancellationToken);
        }

        /// <summary>Activates the wifi asynchronous.</summary>
        /// <param name="profileFilter">The filter to select, which profiles to try to connect to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An async task.</returns>
        public Task TryConnectAsync(Func<string, bool>? profileFilter, CancellationToken cancellationToken)
        {
            if (HasIp())
            {
                return Task.CompletedTask;
            }

            return Task.Run(() => this.ActivateWifi(profileFilter, cancellationToken), cancellationToken);
        }

        private static bool HasIp()
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
            {
                var properties = networkInterface.GetIPProperties();
                foreach (var ipAddressInformation in properties.UnicastAddresses)
                {
                    if (ipAddressInformation.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static async Task<List<string>> GetMatchingWifiProfiles(Func<string, bool>? profileFilter, CancellationToken cancellationToken)
        {
            var processStartInfo = new ProcessStartInfo("netctl", "list");
            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                throw new InvalidOperationException(@$"The ""{processStartInfo.FileName} {processStartInfo.Arguments}"" process could not be started.");
            }

            process.Start();
            cancellationToken.ThrowIfCancellationRequested();
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            var wifiProfiles = new List<string>();
            while (!process.StandardOutput.EndOfStream)
            {
                var wifiProfile = await process.StandardOutput.ReadLineAsync().ConfigureAwait(false);
                if (wifiProfile == null)
                {
                    continue;
                }

                var match = WifiListRegex.Match(wifiProfile);
                if (match.Success)
                {
                    wifiProfile = match.Groups[WifiNameGroupName].Value;
                    if (string.IsNullOrEmpty(wifiProfile) && (profileFilter == null || profileFilter(wifiProfile)))
                    {
                        wifiProfiles.Add(wifiProfile);
                    }
                }
            }

            return wifiProfiles;
        }

        private static void NetctlCommand(string wifiProfile, string command)
        {
            var processStartInfo = new ProcessStartInfo("sudo", $" netctl {command} {wifiProfile}")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            using var process = Process.Start(processStartInfo);

            if (process == null)
            {
                throw new InvalidOperationException(@$"The ""{processStartInfo.FileName} {processStartInfo.Arguments}"" process could not be started.");
            }

            process.Start();
            process.WaitForExit();
        }

        private static bool IsOnline()
        {
            return HasIp();
        }

        private async Task ActivateWifi(Func<string, bool>? profileFilter, CancellationToken cancellationToken)
        {
            try
            {
                var wifiProfiles = await GetMatchingWifiProfiles(profileFilter, cancellationToken).ConfigureAwait(false);
                this.wifiConnecterReporter?.MatchedProfiles(wifiProfiles);
                foreach (var wifiProfile in wifiProfiles)
                {
                    NetctlCommand(wifiProfile, StartCommandText);
                    if (IsOnline())
                    {
                        this.wifiConnecterReporter?.ConnectedToWifi(wifiProfile);
                        return;
                    }

                    this.wifiConnecterReporter?.CouldNotConnectToWifi(wifiProfile);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                this.wifiConnecterReporter?.ConnectionCancelled();
            }
        }
    }
}
