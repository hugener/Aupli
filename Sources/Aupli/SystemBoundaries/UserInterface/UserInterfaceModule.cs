﻿// --------------------------------------------------------------------------------------------------------------------
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
    using Aupli.SystemBoundaries.Bridges.Controls;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Aupli.SystemBoundaries.Bridges.MusicControl;
    using Aupli.SystemBoundaries.Bridges.Timeouts;
    using Aupli.SystemBoundaries.UserInterface.Api;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using Aupli.SystemBoundaries.UserInterface.Display;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Internal;
    using Aupli.SystemBoundaries.UserInterface.Menu;
    using Aupli.SystemBoundaries.UserInterface.Player;
    using Aupli.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using global::Pi.Timers;
    using Sundew.Base.Initialization;
    using Sundew.Pi.ApplicationFramework;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// A module representing the user interface.
    /// </summary>
    public class UserInterfaceModule : IInitializable, IDisposable
    {
        private readonly IApplication application;
        private readonly IUserInterfaceBridge userInterfaceBridge;
        private readonly IControlsModule controlsModule;
        private readonly IMusicPlayer musicPlayer;
        private readonly PlayerModule playerModule;
        private readonly VolumeModule volumeModule;
        private readonly IShutdownParameters shutdownParameters;
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly ITimeoutConfiguration timeoutConfiguration;
        private readonly Reporters reporters;
        private VolumeController volumeController;
        private ShutdownController shutdownController;
        private PlayerController playerController;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceModule" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="userInterfaceBridge">The user interface framework.</param>
        /// <param name="controlsModule">The controls module.</param>
        /// <param name="musicPlayer">The music player.</param>
        /// <param name="playerModule">The player module.</param>
        /// <param name="volumeModule">The volume module.</param>
        /// <param name="shutdownParameters">The shutdown parameters.</param>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        /// <param name="timeoutConfiguration">The timeout configuration.</param>
        /// <param name="reporters">The reporters.</param>
        public UserInterfaceModule(
            IApplication application,
            IUserInterfaceBridge userInterfaceBridge,
            IControlsModule controlsModule,
            IMusicPlayer musicPlayer,
            PlayerModule playerModule,
            VolumeModule volumeModule,
            IShutdownParameters shutdownParameters,
            ILifecycleConfiguration lifecycleConfiguration,
            ITimeoutConfiguration timeoutConfiguration,
            Reporters reporters = null)
        {
            this.application = application;
            this.userInterfaceBridge = userInterfaceBridge;
            this.controlsModule = controlsModule;
            this.musicPlayer = musicPlayer;
            this.playerModule = playerModule;
            this.volumeModule = volumeModule;
            this.shutdownParameters = shutdownParameters;
            this.lifecycleConfiguration = lifecycleConfiguration;
            this.timeoutConfiguration = timeoutConfiguration;
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
            var interactionController =
                new InteractionController(this.controlsModule.InputControls, this.application.InputManager, this.reporters?.InteractionControllerReporter);

            this.application.IdleMonitorReporter = this.reporters?.IdleMonitorReporter;
            var idleMonitor = this.application.CreateIdleMonitoring(
                this.application.InputManager,
                new SystemActivityAggregator(this.musicPlayer, this.reporters?.SystemActivityAggregatorReporter),
                this.timeoutConfiguration.IdleTimeout,
                this.timeoutConfiguration.SystemTimeout);

            this.volumeController = new VolumeController(this.volumeModule.VolumeService, interactionController, this.reporters?.VolumeControllerReporter);

            var menuController = new MenuController(interactionController, this.userInterfaceBridge.TextViewNavigator);

            this.shutdownController = new ShutdownController(
                idleMonitor, this.controlsModule.SystemControl, this.application, this.shutdownParameters.AllowShutdown, this.shutdownParameters.ShutdownCancellationTokenSource, this.reporters?.ShutdownControllerReporter);

            var menuRequester = new MenuRequester();
            this.playerController = new PlayerController(
                interactionController,
                this.playerModule.PlayerService,
                this.musicPlayer,
                menuRequester,
                this.reporters?.PlayerControllerReporter);

            this.ViewNavigator = new ViewNavigator(
                this.volumeModule.VolumeService,
                menuRequester,
                this.shutdownController,
                this.volumeController,
                this.userInterfaceBridge.TextViewNavigator,
                new ViewFactory(this.musicPlayer, this.playerController, this.volumeModule.VolumeService, menuController, this.lifecycleConfiguration),
                new TimerFactory(),
                this.reporters?.ViewNavigatorReporter);

            new DisplayStateController(this.userInterfaceBridge.TextViewNavigator, idleMonitor, this.userInterfaceBridge.Display, this.reporters?.DisplayStateControllerReporter);

            interactionController.Start();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.ViewNavigator.Dispose();
        }
    }
}