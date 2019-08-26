// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartPlaybackFeature.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests
{
    using System.Threading.Tasks;
    using Aupli.DomainServices.Playlist.Shared;
    using Aupli.IntegrationTests.Bootstrapping;
    using global::NSubstitute;
    using global::Serilog;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using MpcNET.Commands.Playback;
    using MpcNET.Commands.Playlist;
    using MpcNET.Message;
    using Sundew.TextView.ApplicationFramework;
    using Xunit;

    public class StartPlaybackFeature
    {
        private readonly ILogger logger = Substitute.For<ILogger>();

        [Fact]
        public async Task Given_TagIsDetectedAndPlaylistExists_Then_PlayCommandShouldBeSent()
        {
            var bootstrapper = new TestBootstrapper(new Application(), this.logger);
            await bootstrapper.StartAsync(true);
            var playlistRepository = bootstrapper.RepositoriesModule.PlaylistRepository;
            playlistRepository.GetPlaylistAsync("01020304").Returns(Task.FromResult(new PlaylistEntity("01020304", "Numbers")));
            var mpcConnection = bootstrapper.MusicControlModule.MpcConnection;
            mpcConnection.SendAsync(Arg.Any<LoadCommand>()).Returns(x =>
            {
                var mpdMessage = Substitute.For<IMpdMessage<string>>();
                mpdMessage.IsResponseValid.Returns(true);
                return Task.FromResult(mpdMessage);
            });
            var rfidTransceiver = (await bootstrapper.ControlsModuleFactory.ControlsModule).InputControls.RfidTransceiver;

            rfidTransceiver.TagDetected += Raise.EventWith(new TagDetectedEventArgs(new Uid(1, 2, 3, 4)));

            await mpcConnection.Received(1).SendAsync(Arg.Any<PlayCommand>());
        }
    }
}