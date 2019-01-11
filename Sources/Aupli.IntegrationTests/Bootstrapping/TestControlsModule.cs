// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestControlsModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System;
    using SystemBoundaries.Bridges.Interaction;
    using SystemBoundaries.Pi;
    using SystemBoundaries.Pi.Amplifier.Api;
    using SystemBoundaries.Pi.Amplifier.Ari;
    using SystemBoundaries.Pi.SystemControl.Api;
    using global::NSubstitute;
    using NSubstitute;
    using Pi.IO.GeneralPurpose;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;

    public class TestControlsModule : ControlsModule
    {
        public TestControlsModule(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            IAmplifierReporter amplifierReporter)
            : base(gpioConnectionDriverFactory, amplifierReporter)
        {
        }

        protected override IAmplifierFactory CreateAmplifierFactory()
        {
            var amplifier = Substitute.For<IAmplifierFactory>();
            amplifier.Create(Arg.Any<IAmplifierReporter>()).ReturnsSubstitute();
            return amplifier;
        }

        protected override InputControls CreateInputControls(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new InputControls(
                Substitute.For<IButtonDevice>(),
                Substitute.For<IButtonDevice>(),
                Substitute.For<IButtonDevice>(),
                Substitute.For<IButtonDevice>(),
                Substitute.For<IRfidConnection>(),
                Substitute.For<ILircDevice>(),
                Substitute.For<IRotaryEncoderDevice>(),
                Substitute.For<IDisposable>());
        }

        protected override ISystemControlFactory CreateSystemControlFactory()
        {
            var systemControlFactory = Substitute.For<ISystemControlFactory>();
            systemControlFactory.Create(Arg.Any<IGpioConnectionDriverFactory>()).ReturnsSubstitute();
            return systemControlFactory;
        }
    }
}