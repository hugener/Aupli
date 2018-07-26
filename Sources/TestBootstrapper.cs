// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Aupli.IntegrationTests
{
    using Aupli.Bootstrapping;
    using Aupli.Domain.Playlist;
    using Aupli.Domain.Volume;
    using Aupli.SystemBoundaries.Persistence.Configuration;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;
    using Telerik.JustMock;
    using Aupli.Logging.Serilog.Mpc;
    using MpcNET;

    public class TestBootstrapper : Bootstrapper
    {
        public TestBootstrapper(IGpioConnectionDriver gpioConnectionDriver, ILogger logger)
            : base(gpioConnectionDriver, logger)
        {
        }

        public InputControls InputControls { get; private set; }

        public IMpcConnection MpcConnection { get; private set; }

        protected override IConfigurationRepository CreateConfigurationRepository()
        {
            return Mock.Create<IConfigurationRepository>();
        }

        protected override Repositories CreateRepositories()
        {
            return new Repositories(
                Mock.Create<IVolumeRepository>(),
                Mock.Create<IPlaylistRepository>(),
                Mock.Create<ILastPlaylistRepository>());
        }

        protected override InputControls CreateInputControls(IGpioConnectionDriver gpioConnectionDriver)
        {
            return this.InputControls = new InputControls(
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IRfidConnection>(),
                Mock.Create<ILircDevice>(),
                Mock.Create<IRotaryEncoderDevice>());
        }

        protected override IMpcConnection CreateMpcConnection(MusicPlayerLogger musicPlayerLogger)
        {
            return this.MpcConnection = Mock.Create<IMpcConnection>();
        }
    }
}