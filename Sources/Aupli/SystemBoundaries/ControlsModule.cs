// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlsModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Amplifier;
    using Aupli.SystemBoundaries.Connectors.System;
    using Aupli.SystemBoundaries.Mpc;
    using Aupli.SystemBoundaries.Pi.Amplifier;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Aupli.SystemBoundaries.Pi.SystemControl;
    using global::MpcNET;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Base.Initialization;

    /// <summary>
    /// The user interface module.
    /// </summary>
    public class ControlsModule : IInitializable, IDisposable
    {
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly RequiredInterface.IMusicPlayerReporter musicPlayerReporter;
        private readonly IAmplifierReporter amplifierReporter;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlsModule" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="musicPlayerReporter">The music player reporter.</param>
        /// <param name="amplifierReporter">The amplifier reporter.</param>
        public ControlsModule(IGpioConnectionDriver gpioConnectionDriver, RequiredInterface.IMusicPlayerReporter musicPlayerReporter, IAmplifierReporter amplifierReporter)
        {
            this.gpioConnectionDriver = gpioConnectionDriver;
            this.musicPlayerReporter = musicPlayerReporter;
            this.amplifierReporter = amplifierReporter;
        }

        /// <summary>
        /// Gets the music player.
        /// </summary>
        /// <value>
        /// The music player.
        /// </value>
        public IMusicPlayer MusicPlayer { get; private set; }

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
            this.InputControls = this.CreateInputControls(this.gpioConnectionDriver);

            var systemControlFactory = this.CreateSystemControlFactory();
            this.SystemControl = systemControlFactory.Create(this.gpioConnectionDriver);

            var amplifierFactory = this.CreateAmplifierFactory();
            this.Amplifier = amplifierFactory.Create(this.amplifierReporter);

            // Create external services
            var mpcConnection = this.CreateMpcConnection(this.musicPlayerReporter);
            this.MusicPlayer = new MusicPlayer(mpcConnection, this.musicPlayerReporter);
            this.disposer = new Disposer(
                systemControlFactory,
                amplifierFactory,
                this.InputControls.RemoteControl,
                this.InputControls.RfidTransceiver,
                this.InputControls.RotaryEncoder,
                new GpioConnection(
                    new[]
                    {
                        this.InputControls.PlayPauseButton.PinConfiguration,
                        this.InputControls.NextButton.PinConfiguration,
                        this.InputControls.PreviousButton.PinConfiguration,
                        this.InputControls.MenuButton.PinConfiguration,
                    }.Where(x => x != null)),
                mpcConnection,
                this.MusicPlayer);
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
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <returns>
        /// A input controls.
        /// </returns>
        protected virtual InputControls CreateInputControls(IGpioConnectionDriver gpioConnectionDriver)
        {
            return InputControlsFactory.Create(gpioConnectionDriver);
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

        /// <summary>
        /// Creates the MPC connection.
        /// </summary>
        /// <param name="mpcConnectionReporter">The music player logger.</param>
        /// <returns>A mpc connection.</returns>
        protected virtual IMpcConnection CreateMpcConnection(IMpcConnectionReporter mpcConnectionReporter)
        {
            return new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600), mpcConnectionReporter);
        }
    }
}