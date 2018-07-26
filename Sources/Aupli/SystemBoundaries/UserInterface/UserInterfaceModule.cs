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
    using Aupli.Bootstrapping;
    using Aupli.SystemBoundaries.Connectors.Lifecycle;
    using Aupli.SystemBoundaries.Connectors.Timeouts;
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Menu;
    using Aupli.SystemBoundaries.UserInterface.Player;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using Aupli.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using global::Pi.Timers;
    using Sundew.Base.Disposal;
    using Sundew.Base.Initialization;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// A module representing the user interface.
    /// </summary>
    public class UserInterfaceModule : IInitializable, IDisposable
    {
        private readonly IdleController idleController;
        private readonly Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceModule" /> class.
        /// </summary>
        /// <param name="controlsModule">The controls module.</param>
        /// <param name="playerModule">The player module.</param>
        /// <param name="volumeModule">The volume module.</param>
        /// <param name="inputParameters">The startup module.</param>
        /// <param name="shutdownParameters">The shutdown parameters.</param>
        /// <param name="timeoutConfiguration">The timeout configuration.</param>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        /// <param name="reporters">The reporters.</param>
        public UserInterfaceModule(
            ControlsModule controlsModule,
            PlayerModule playerModule,
            VolumeModule volumeModule,
            IInputParameters inputParameters,
            IShutdownParameters shutdownParameters,
            ITimeoutConfiguration timeoutConfiguration,
            ILifecycleConfiguration lifecycleConfiguration,
            Reporters reporters = null)
        {
            // Create user interface
            var interactionController =
                new InteractionController(controlsModule.InputControls, inputParameters.InputManager, reporters?.InteractionControllerReporter);

            this.idleController = new IdleController(
                interactionController,
                new SystemActivityAggregator(controlsModule.MusicPlayer, reporters?.SystemActivityAggregatorReporter),
                timeoutConfiguration.IdleTimeout,
                timeoutConfiguration.SystemTimeout,
                reporters?.IdleControllerReporter);

            this.PlayerController = new PlayerController(
                interactionController,
                playerModule.PlayerService,
                controlsModule.MusicPlayer,
                reporters?.PlayerControllerReporter);

            this.VolumeController = new VolumeController(
                volumeModule.VolumeService,
                interactionController,
                reporters?.VolumeControllerReporter);

            var menuController = new MenuController(interactionController, inputParameters.TextViewNavigator);

            this.ShutdownController = new ShutdownController(
                this.idleController,
                controlsModule.SystemControl,
                shutdownParameters.AllowShutdown,
                shutdownParameters.ShutdownCancellationTokenSource,
                reporters?.ShutdownControllerReporter);

            this.ViewNavigator = new ViewNavigator(
                volumeModule.VolumeService,
                this.PlayerController,
                this.ShutdownController,
                inputParameters.TextViewNavigator,
                new ViewFactory(controlsModule.MusicPlayer, this.PlayerController, this.VolumeController, volumeModule.VolumeService, menuController, lifecycleConfiguration),
                new TimerFactory(),
                reporters?.ViewNavigatorReporter);

            new DisplayBackLightController(this.idleController, inputParameters.Display, reporters?.DisplayBacklightControllerReporter);
            this.disposer = new Disposer(this.idleController, this.ViewNavigator);
        }

        /// <summary>
        /// Gets the player controller.
        /// </summary>
        /// <value>
        /// The player controller.
        /// </value>
        public PlayerController PlayerController { get; }

        /// <summary>
        /// Gets the volume controller.
        /// </summary>
        /// <value>
        /// The volume controller.
        /// </value>
        public VolumeController VolumeController { get; }

        /// <summary>
        /// Gets the shutdown controller.
        /// </summary>
        /// <value>
        /// The shutdown controller.
        /// </value>
        public ShutdownController ShutdownController { get; }

        /// <summary>
        /// Gets the view navigator.
        /// </summary>
        /// <value>
        /// The view navigator.
        /// </value>
        public ViewNavigator ViewNavigator { get; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
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
    }
}