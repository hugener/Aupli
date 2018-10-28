﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlsModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
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
    using Sundew.Base.Initialization;

    /// <summary>
    /// The user interface module.
    /// </summary>
    public class ControlsModule : IControlsModule, IInitializable, IDisposable
    {
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly IAmplifierReporter amplifierReporter;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsModule" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver.</param>
        /// <param name="amplifierReporter">The amplifier reporter.</param>
        public ControlsModule(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            IAmplifierReporter amplifierReporter)
        {
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.amplifierReporter = amplifierReporter;
        }

        /// <summary>
        /// Gets the amplifier.
        /// </summary>
        /// <value>
        /// The amplifier.
        /// </value>
        public IAmplifier Amplifier { get; private set; }

        /// <summary>
        /// Gets the system control.
        /// </summary>
        /// <value>
        /// The system control.
        /// </value>
        public ISystemControl SystemControl { get; private set; }

        /// <summary>
        /// Gets the input controls.
        /// </summary>
        /// <value>
        /// The input controls.
        /// </value>
        public InputControls InputControls { get; private set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public Task InitializeAsync()
        {
            // Create Hardware
            this.InputControls = this.CreateInputControls(this.gpioConnectionDriverFactory);

            var systemControlFactory = this.CreateSystemControlFactory();
            this.SystemControl = systemControlFactory.Create(this.gpioConnectionDriverFactory);

            var amplifierFactory = this.CreateAmplifierFactory();
            this.Amplifier = amplifierFactory.Create(this.amplifierReporter);

            // Create external services
            this.disposer = new Disposer(
                systemControlFactory,
                amplifierFactory,
                this.InputControls.RemoteControl,
                this.InputControls.RfidTransceiver,
                this.InputControls.RotaryEncoder,
                new GpioConnection(
                    this.gpioConnectionDriverFactory,
                    new[]
                    {
                        this.InputControls.PlayPauseButton.PinConfiguration,
                        this.InputControls.NextButton.PinConfiguration,
                        this.InputControls.PreviousButton.PinConfiguration,
                        this.InputControls.MenuButton.PinConfiguration,
                    }.Where(x => x != null)));

            return Task.CompletedTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.disposer?.Dispose();
            this.disposer = null;
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
    }
}