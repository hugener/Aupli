// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartPlaybackFeature.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Bootstrapping;
    using DomainServices.Playlist.Shared;
    using global::Serilog;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using MpcNET.Commands.Playback;
    using Sundew.TextView.ApplicationFramework;
    using Telerik.JustMock;
    using Telerik.JustMock.Helpers;
    using Xunit;

    public class StartPlaybackFeature
    {
        private readonly ILogger logger = Mock.Create<ILogger>();

        [Fact]
        public async Task Given_TagIsDetectedAndPlaylistExists_Then_PlayCommandShouldBeSent()
        {
            var bootstrapper = new TestBootstrapper(new Application(), this.logger);
            await bootstrapper.StartAsync(true);
            Mock.Arrange(() => bootstrapper.RepositoriesModule.PlaylistRepository.GetPlaylistAsync("1234"))
                .Returns(Task.FromResult(new PlaylistEntity("1234", "Numbers")));
            var rfidTransceiver =
                (await bootstrapper.ControlsModuleFactory.ControlsModule).InputControls.RfidTransceiver;

            rfidTransceiver.Raise(x => x.TagDetected += null, new TagDetectedEventArgs(new Uid(1, 2, 3, 4)));

            Mock.Assert(() => bootstrapper.MusicControlModule.MpcConnection.SendAsync(Arg.IsAny<PlayCommand>()),
                Occurs.Once());
        }
    }
}