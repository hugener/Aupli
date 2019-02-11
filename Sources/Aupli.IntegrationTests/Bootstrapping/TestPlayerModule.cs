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
    using SystemBoundaries.SystemServices.Api;
    using SystemBoundaries.SystemServices.Ari;
    using ApplicationServices;
    using ApplicationServices.Player.Ari;
    using DomainServices.Playlist.Api;
    using Telerik.JustMock;

    public class TestPlayerModule : PlayerModule
    {
        public TestPlayerModule(IPlaylistRepository playlistRepository, ILastPlaylistService lastPlaylistService, IPlaybackControls playbackControls, ISystemServicesAwaiterReporter systemServicesAwaiterReporter, IPlayerServiceReporter playerServiceReporter) 
            : base(playlistRepository, lastPlaylistService, playbackControls, systemServicesAwaiterReporter, playerServiceReporter)
        {
        }

        protected override ISystemServicesAwaiter CreateServicesAwaiter()
        {
            var systemServicesAwaiter = Mock.Create<ISystemServicesAwaiter>();
            Mock.Arrange(() => systemServicesAwaiter.WaitForServicesAsync(Arg.IsAny<IEnumerable<string>>(), Timeout.InfiniteTimeSpan)).Returns(Task.FromResult(true));
            return systemServicesAwaiter;
        }
    }
}