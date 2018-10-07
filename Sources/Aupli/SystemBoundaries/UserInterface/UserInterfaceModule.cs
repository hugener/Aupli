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
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Aupli.SystemBoundaries.Bridges.Timeouts;
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Api;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Internal;
    using Aupli.SystemBoundaries.UserInterface.Menu;
    using Aupli.SystemBoundaries.UserInterface.Player;
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
        private readonly IUserInterfaceBridge userInterfaceBridge;
        private readonly IControlsModule controlsModule;
        private readonly PlayerModule playerModule;
        private readonly VolumeModule volumeModule;
        private readonly IShutdownParameters shutdownParameters;
        private readonly ITimeoutConfiguration timeoutConfiguration;
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly Reporters reporters;
        private IdleController idleController;
        private Disposer disposer;
        private VolumeController volumeController;
        private ShutdownController shutdownController;
        private PlayerController playerController;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceModule" /> class.
        /// </summary>
        /// <param name="userInterfaceBridge">The user interface framework.</param>
        /// <param name="controlsModule">The controls module.</param>
        /// <param name="playerModule">The player module.</param>
        /// <param name="volumeModule">The volume module.</param>
        /// <param name="shutdownParameters">The shutdown parameters.</param>
        /// <param name="timeoutConfiguration">The timeout configuration.</param>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        /// <param name="reporters">The reporters.</param>
        public UserInterfaceModule(
            IUserInterfaceBridge userInterfaceBridge,
            IControlsModule controlsModule,
            PlayerModule playerModule,
            VolumeModule volumeModule,
            IShutdownParameters shutdownParameters,
            ITimeoutConfiguration timeoutConfiguration,
            ILifecycleConfiguration lifecycleConfiguration,
            Reporters reporters = null)
        {
            this.userInterfaceBridge = userInterfaceBridge;
            this.controlsModule = controlsModule;
            this.playerModule = playerModule;
            this.volumeModule = volumeModule;
            this.shutdownParameters = shutdownParameters;
            this.timeoutConfiguration = timeoutConfiguration;
            this.lifecycleConfiguration = lifecycleConfiguration;
            this.reporters = reporters;
        }

        /// <summary>
        /// Gets the view navigator.
        /// </summary>
        /// <value>
        /// The view navigator.
        /// </value>
        public IViewNavigator ViewNavigator { get; private set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            // Create user interface
            var interactionController =
                new InteractionController(this.controlsModule.InputControls, this.userInterfaceBridge.InputManager, this.reporters?.InteractionControllerReporter);

            this.idleController = new IdleController(
                interactionController,
                new SystemActivityAggregator(this.controlsModule.MusicPlayer, this.reporters?.SystemActivityAggregatorReporter),
                this.timeoutConfiguration.IdleTimeout,
                this.timeoutConfiguration.SystemTimeout,
                this.reporters?.IdleControllerReporter);

            this.volumeController = new VolumeController(this.volumeModule.VolumeService, interactionController, this.reporters?.VolumeControllerReporter);

            var menuController = new MenuController(interactionController, this.userInterfaceBridge.TextViewNavigator);

            this.shutdownController = new ShutdownController(
                this.idleController, this.controlsModule.SystemControl, this.shutdownParameters.AllowShutdown, this.shutdownParameters.ShutdownCancellationTokenSource, this.reporters?.ShutdownControllerReporter);

            var menuRequester = new MenuRequester();
            this.playerController = new PlayerController(
                interactionController,
                this.playerModule.PlayerService,
                this.controlsModule.MusicPlayer,
                menuRequester,
                this.reporters?.PlayerControllerReporter);

            this.ViewNavigator = new ViewNavigator(
                this.volumeModule.VolumeService,
                menuRequester,
                this.shutdownController,
                this.userInterfaceBridge.TextViewNavigator,
                new ViewFactory(this.controlsModule.MusicPlayer, this.playerController, this.volumeController, this.volumeModule.VolumeService, menuController, this.lifecycleConfiguration),
                new TimerFactory(),
                this.reporters?.ViewNavigatorReporter);

            new DisplayStateController(this.idleController, this.userInterfaceBridge.Display, this.reporters?.DisplayStateControllerReporter);
            this.disposer = new Disposer(this.idleController, this.ViewNavigator);
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