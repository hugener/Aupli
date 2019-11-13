// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestPlayerModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using Aupli.ApplicationServices;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.DomainServices.Playlist.Api;

    public class TestPlayerModule : PlayerModule
    {
        public TestPlayerModule(IPlaylistRepository playlistRepository, ILastPlaylistService lastPlaylistService, IPlaybackControls playbackControls, IPlayerServiceReporter playerServiceReporter)
            : base(playlistRepository, lastPlaylistService, playbackControls, playerServiceReporter)
        {
        }
    }
}