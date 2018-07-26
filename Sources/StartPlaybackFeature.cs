// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartPlaybackFeature.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::Pi.IO.GeneralPurpose;
    using global::Serilog;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers;
    using global::Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;
    using MpcNET;
    using Telerik.JustMock;
    using Xunit;

    public class StartPlaybackFeature
    {
        private readonly IGpioConnectionDriver gpioConnectionDriver = Mock.Create<IGpioConnectionDriver>();
        private readonly ILogger logger = Mock.Create<ILogger>();

        [Fact]
        public async Task T()
        {
            var bootstrapper = new TestBootstrapper(this.gpioConnectionDriver, this.logger);
            var commandFactory = Mock.Create<ICommandFactory>();
            await bootstrapper.StartAsync(new CancellationTokenSource(), true);
            var mpcConnection = bootstrapper.MpcConnection;
            var rfidTransceiver = bootstrapper.InputControls.RfidTransceiver;
            Mock.Arrange(() => mpcConnection.SendAsync(Arg.Matches<Func<ICommandFactory, IMpcCommand<string>>>(x => x(commandFactory).GetType().Name == "LoadCommand")));

            Mock.Raise(
                () => rfidTransceiver.TagDetected += null,
                new TagDetectedEventArgs(new Uid(0, 0, 0, 0)));

            Mock.Assert(mpcConnection);
        }
    }
}