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
    using global::Serilog;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using global::Telerik.JustMock;
    using Moq;
    using MpcNET.Commands.Playback;
    using MpcNET.Commands.Playlist;
    using MpcNET.Message;
    using Sundew.TextView.ApplicationFramework;
    using Xunit;

    public class StartPlaybackFeature
    {
        private readonly ILogger logger = New.Mock<ILogger>().SetDefaultValue(DefaultValue.Mock);

        [Fact]
        public async Task Given_PlaylistExists_When_TagIsDetected_Then_PlayCommandShouldBeSent()
        {
            var bootstrapper = new TestBootstrapper(new Application(), this.logger);
            await bootstrapper.StartAsync(true).ConfigureAwait(false);
            var playlistRepository = bootstrapper.RepositoriesModule.PlaylistRepository;
            playlistRepository.Setup(x => x.GetPlaylistAsync("01020304")).ReturnsAsync(new PlaylistEntity("01020304", "Numbers"));
            var mpcConnection = bootstrapper.MusicControlModule.MpcConnection;
            mpcConnection.Setup(x => x.SendAsync(It.IsAny<LoadCommand>())).ReturnsAsync(() =>
            {
                var mpdMessage = New.Mock<IMpdMessage<string>>();
                mpdMessage.SetupGet(x => x.IsResponseValid).Returns(true);
                return mpdMessage;
            });
            var rfidTransceiver = (await bootstrapper.ControlsModuleFactory.ControlsModule.ConfigureAwait(false)).InputControls.RfidTransceiver;

            rfidTransceiver.Raise(x => x.TagDetected += null, new TagDetectedEventArgs(new Uid(1, 2, 3, 4)));

            mpcConnection.Verify(x => x.SendAsync(It.IsAny<PlayCommand>()), Times.Once);
        }
    }
}