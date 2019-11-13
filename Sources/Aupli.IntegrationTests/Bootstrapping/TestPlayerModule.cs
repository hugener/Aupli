// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestPlayerModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.DomainServices.Playlist.Api;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using global::NSubstitute;

    public class TestPlayerModule : PlayerModule
    {
        public TestPlayerModule(IPlaylistRepository playlistRepository, ILastPlaylistService lastPlaylistService, IPlaybackControls playbackControls, ISystemServicesAwaiterReporter systemServicesAwaiterReporter, IPlayerServiceReporter playerServiceReporter, IWifiConnecterReporter wifiConnecterReporter)
            : base(playlistRepository, lastPlaylistService, playbackControls, systemServicesAwaiterReporter, playerServiceReporter, wifiConnecterReporter)
        {
        }

        protected override ISystemServicesAwaiter CreateServicesAwaiter()
        {
            var systemServicesAwaiter = Substitute.For<ISystemServicesAwaiter>();
            systemServicesAwaiter.WaitForServicesAsync(Arg.Any<IEnumerable<string>>(), Timeout.InfiniteTimeSpan).Returns(Task.FromResult(true));
            return systemServicesAwaiter;
        }
    }
}