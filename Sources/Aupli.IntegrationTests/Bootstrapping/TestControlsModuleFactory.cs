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
    using JustMock;
    using Pi.IO.GeneralPurpose;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;
    using Telerik.JustMock;

    public class TestControlsModuleFactory : ControlsModuleFactory
    {
        public TestControlsModuleFactory(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            IAmplifierReporter amplifierReporter)
            : base(gpioConnectionDriverFactory, amplifierReporter)
        {
        }

        protected override InputControls CreateInputControls(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new InputControls(
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IButtonDevice>(),
                Mock.Create<IRfidConnection>(),
                Mock.Create<ILircDevice>(),
                Mock.Create<IRotaryEncoderWithButtonDevice>(),
                Mock.Create<IDisposable>());
        }
    }
}