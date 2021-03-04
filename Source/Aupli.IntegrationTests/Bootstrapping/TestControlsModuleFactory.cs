// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestControlsModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Pi;
    using Aupli.SystemBoundaries.Pi.Amplifier.Api;
    using Aupli.SystemBoundaries.Pi.Amplifier.Ari;
    using Aupli.SystemBoundaries.Pi.SystemControl.Api;
    using Moq;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;

    public class TestControlsModuleFactory : ControlsModuleFactory
    {
        public TestControlsModuleFactory(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            IAmplifierReporter amplifierReporter,
            IDisposableReporter disposableReporter)
            : base(gpioConnectionDriverFactory, amplifierReporter, disposableReporter)
        {
        }

        protected override InputControls CreateInputControls(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new InputControls(
                New.Mock<IButtonDevice>(),
                New.Mock<IButtonDevice>(),
                New.Mock<IButtonDevice>(),
                New.Mock<IButtonDevice>(),
                New.Mock<IRfidConnection>(),
                New.Mock<ILircDevice>(),
                New.Mock<IRotaryEncoderWithButtonDevice>(),
                New.Mock<IDisposable>());
        }

        protected override ISystemControlFactory CreateSystemControlFactory()
        {
            return New.Mock<ISystemControlFactory>().SetDefaultValue(DefaultValue.Mock);
        }

        protected override IAmplifierFactory CreateAmplifierFactory()
        {
            return New.Mock<IAmplifierFactory>().SetDefaultValue(DefaultValue.Mock);
        }
    }
}