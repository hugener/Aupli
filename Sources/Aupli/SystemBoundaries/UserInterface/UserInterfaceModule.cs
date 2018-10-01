// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserInterfaceModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface
{
    using System;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices;
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Aupli.SystemBoundaries.Shared.Lifecycle;
    using Aupli.SystemBoundaries.Shared.Timeouts;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Internal;
    using Aupli.SystemBoundaries.UserInterface.Menu;
    using Aupli.SystemBoundaries.UserInterface.Player;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using Aupli.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using global::Pi.IO.GeneralPurpose;
    using global::Pi.Timers;
    using Sundew.Base.Disposal;
    using Sundew.Base.Initialization;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Navigation;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// A module representing the user interface.
    /// </summary>
    public class UserInterfaceModule : IInitializable, IDisposable
    {
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly ControlsModule controlsModule;
        private readonly PlayerModule playerModule;
        private readonly VolumeModule volumeModule;
        private readonly IShutdownParameters shutdownParameters;
        private readonly ITimeoutConfiguration timeoutConfiguration;
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly Reporters reporters;
        private IdleController idleController;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceModule" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="controlsModule">The controls module.</param>
        /// <param name="playerModule">The player module.</param>
        /// <param name="volumeModule">The volume module.</param>
        /// <param name="shutdownParameters">The shutdown parameters.</param>
        /// <param name="timeoutConfiguration">The timeout configuration.</param>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        /// <param name="reporters">The reporters.</param>
        public UserInterfaceModule(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            ControlsModule controlsModule,
            PlayerModule playerModule,
            VolumeModule volumeModule,
            IShutdownParameters shutdownParameters,
            ITimeoutConfiguration timeoutConfiguration,
            ILifecycleConfiguration lifecycleConfiguration,
            Reporters reporters = null)
        {
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.controlsModule = controlsModule;
            this.playerModule = playerModule;
            this.volumeModule = volumeModule;
            this.shutdownParameters = shutdownParameters;
            this.timeoutConfiguration = timeoutConfiguration;
            this.lifecycleConfiguration = lifecycleConfiguration;
            this.reporters = reporters;
        }

        /// <summary>
        /// Gets the player controller.
        /// </summary>
        /// <value>
        /// The player controller.
        /// </value>
        public PlayerController PlayerController { get; private set; }

        /// <summary>
        /// Gets the volume controller.
        /// </summary>
        /// <value>
        /// The volume controller.
        /// </value>
        public VolumeController VolumeController { get; private set; }

        /// <summary>
        /// Gets the shutdown controller.
        /// </summary>
        /// <value>
        /// The shutdown controller.
        /// </value>
        public ShutdownController ShutdownController { get; private set; }

        /// <summary>
        /// Gets the view navigator.
        /// </summary>
        /// <value>
        /// The view navigator.
        /// </value>
        public ViewNavigator ViewNavigator { get; private set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            // Create display
            var displayFactory = this.CreateDisplayFactory();
            var display = displayFactory.Create(this.gpioConnectionDriverFactory, this.lifecycleConfiguration.Pin26Feature == Pin26Feature.Backlight);

            // Create Text Rendering
            var textViewRendererFactory = new TextViewRendererFactory(display, this.reporters?.TextViewRendererReporter);
            var textViewRenderer = textViewRendererFactory.Create();

            // Show welcome/loading message
            var inputManager = new InputManager(this.reporters?.InputManagerReporter);
            var textViewNavigator = new TextViewNavigator(textViewRenderer, inputManager);

            textViewRenderer.Start();

            // Create user interface
            var interactionController =
                new InteractionController(this.controlsModule.InputControls, inputManager, this.reporters?.InteractionControllerReporter);

            this.idleController = new IdleController(
                interactionController,
                new SystemActivityAggregator(this.controlsModule.MusicPlayer, this.reporters?.SystemActivityAggregatorReporter),
                this.timeoutConfiguration.IdleTimeout,
                this.timeoutConfiguration.SystemTimeout,
                this.reporters?.IdleControllerReporter);

            this.VolumeController = new VolumeController(this.volumeModule.VolumeService, interactionController, this.reporters?.VolumeControllerReporter);

            var menuController = new MenuController(interactionController, textViewNavigator);

            this.ShutdownController = new ShutdownController(
                this.idleController, this.controlsModule.SystemControl, this.shutdownParameters.AllowShutdown, this.shutdownParameters.ShutdownCancellationTokenSource, this.reporters?.ShutdownControllerReporter);

            var menuRequester = new MenuRequester();
            this.PlayerController = new PlayerController(
                interactionController,
                this.playerModule.PlayerService,
                this.controlsModule.MusicPlayer,
                menuRequester,
                this.reporters?.PlayerControllerReporter);

            this.ViewNavigator = new ViewNavigator(
                this.volumeModule.VolumeService,
                menuRequester,
                this.ShutdownController,
                textViewNavigator,
                new ViewFactory(this.controlsModule.MusicPlayer, this.PlayerController, this.VolumeController, this.volumeModule.VolumeService, menuController, this.lifecycleConfiguration),
                new TimerFactory(),
                this.reporters?.ViewNavigatorReporter);

            new DisplayStateController(this.idleController, display, this.reporters?.DisplayStateControllerReporter);
            this.disposer = new Disposer(this.idleController, this.ViewNavigator, textViewRenderer, displayFactory);
            this.idleController.Start();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.disposer.Dispose();
        }

        /// <summary>
        /// Creates the display factory.
        /// </summary>
        /// <returns>A Display Factory.</returns>
        protected virtual DisplayFactory CreateDisplayFactory()
        {
            return new DisplayFactory();
        }
    }
}