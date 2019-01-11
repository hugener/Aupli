// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartPlaybackFeature.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests
{
    using System.Threading.Tasks;
    using Bootstrapping;
    using global::NSubstitute;
    using global::Serilog;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using MpcNET.Commands.Playback;
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

            bootstrapper.ControlsModule.InputControls.RfidTransceiver.TagDetected += Raise.EventWith(new TagDetectedEventArgs(new Uid(0, 0, 0, 0)));

            await bootstrapper.MusicControlModule.MpcConnection.Received(1).SendAsync(Arg.Any<PlayCommand>());
        }
    }
}