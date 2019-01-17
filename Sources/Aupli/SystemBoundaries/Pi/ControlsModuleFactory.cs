// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlsModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi
{
    using System;
    using Aupli.ApplicationServices.Volume.Ari;
    using Aupli.SystemBoundaries.Bridges.Controls;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Shutdown;
    using Aupli.SystemBoundaries.Pi.Amplifier;
    using Aupli.SystemBoundaries.Pi.Amplifier.Api;
    using Aupli.SystemBoundaries.Pi.Amplifier.Ari;
    using Aupli.SystemBoundaries.Pi.Input;
    using Aupli.SystemBoundaries.Pi.SystemControl;
    using Aupli.SystemBoundaries.Pi.SystemControl.Api;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Base.Threading;

    /// <summary>
    /// The user interface module.
    /// </summary>
    public class ControlsModuleFactory : IControlsModuleFactory
    {
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly IAmplifierReporter amplifierReporter;
        private readonly AsyncLazy<IControlsModule, PrivateControlModule> controlsModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsModuleFactory" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver.</param>
        /// <param name="amplifierReporter">The amplifier reporter.</param>
        public ControlsModuleFactory(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            IAmplifierReporter amplifierReporter)
        {
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.amplifierReporter = amplifierReporter;
            this.controlsModule = new AsyncLazy<IControlsModule, PrivateControlModule>(() =>
            {
                // Create Hardware
                var inputControls = this.CreateInputControls(this.gpioConnectionDriverFactory);

                var systemControlFactory = this.CreateSystemControlFactory();
                var systemControl = systemControlFactory.Create(this.gpioConnectionDriverFactory);

                var amplifierFactory = this.CreateAmplifierFactory();
                var amplifier = amplifierFactory.Create(this.amplifierReporter);

                var disposer = new Disposer(
                    systemControlFactory,
                    amplifierFactory,
                    inputControls.RemoteControl,
                    inputControls.RfidTransceiver,
                    inputControls.RotaryEncoder,
                    inputControls.ButtonsGpioConnection);
                return new PrivateControlModule(inputControls, systemControl, amplifier, disposer);
            });
        }

        /// <summary>
        /// Gets the controls module.
        /// </summary>
        /// <value>
        /// The system control.
        /// </value>
        public IAsyncLazy<IControlsModule> ControlsModule { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.controlsModule.GetValueOrDefault()?.Disposer.Dispose();
        }

        /// <summary>
        /// Creates the input controls.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>
        /// A input controls.
        /// </returns>
        protected virtual InputControls CreateInputControls(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return InputControlsFactory.Create(gpioConnectionDriverFactory);
        }

        /// <summary>
        /// Creates the system control factory.
        /// </summary>
        /// <returns>A system control factory.</returns>
        protected virtual ISystemControlFactory CreateSystemControlFactory()
        {
            return new SystemControlFactory();
        }

        /// <summary>
        /// Creates the amplifier factory.
        /// </summary>
        /// <returns>An amplifier factory.</returns>
        protected virtual IAmplifierFactory CreateAmplifierFactory()
        {
            return new AmplifierFactory();
        }

        private class PrivateControlModule : IControlsModule
        {
            public PrivateControlModule(InputControls inputControls, ISystemControl systemControl, IAmplifier amplifier, IDisposable disposer)
            {
                this.Amplifier = amplifier;
                this.SystemControl = systemControl;
                this.InputControls = inputControls;
                this.Disposer = disposer;
            }

            public InputControls InputControls { get; }

            public ISystemControl SystemControl { get; }

            public IAmplifier Amplifier { get; }

            public IDisposable Disposer { get; }
        }
    }
}