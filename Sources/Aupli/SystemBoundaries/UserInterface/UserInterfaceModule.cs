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
    using global::Pi.Core.Timers;
    using Sundew.Base.Disposal;
    using Sundew.Base.Initialization;
    using Sundew.TextView.ApplicationFramework;

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
        private readonly bool allowShutdown;
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly ITimeoutConfiguration timeoutConfiguration;
        private readonly Reporters? reporters;
        private VolumeController volumeController = default!;
        private ShutdownController shutdownController = default!;
        private PlayerController playerController = default!;
        private Disposer disposer = default!;
        private IViewNavigator? viewNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceModule" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="userInterfaceBridge">The user interface framework.</param>
        /// <param name="controlsModule">The controls module.</param>
        /// <param name="musicPlayer">The music player.</param>
        /// <param name="playerModule">The player module.</param>
        /// <param name="volumeModule">The volume module.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
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
            bool allowShutdown,
            ILifecycleConfiguration lifecycleConfiguration,
            ITimeoutConfiguration timeoutConfiguration,
            Reporters? reporters = null)
        {
            this.application = application;
            this.userInterfaceBridge = userInterfaceBridge;
            this.controlsModule = controlsModule;
            this.musicPlayer = musicPlayer;
            this.playerModule = playerModule;
            this.volumeModule = volumeModule;
            this.allowShutdown = allowShutdown;
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
        public IViewNavigator ViewNavigator
        {
            get => this.viewNavigator ?? throw new NotInitializedException(this);
            private set => this.viewNavigator = value;
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async ValueTask InitializeAsync()
        {
            var interactionController =
                new InteractionController(this.controlsModule.InputControls, this.application.InputManager, this.reporters?.InteractionControllerReporter);

            this.application.IdleMonitorReporter = this.reporters?.IdleMonitorReporter;
            var idleMonitor = this.application.CreateIdleMonitoring(
                null,
                new SystemActivityAggregator(this.musicPlayer, this.reporters?.SystemActivityAggregatorReporter),
                this.timeoutConfiguration.IdleTimeout,
                this.timeoutConfiguration.SystemTimeout);

            this.volumeController = new VolumeController(this.volumeModule.VolumeService, interactionController, this.reporters?.VolumeControllerReporter);

            var menuController = new MenuController(interactionController, this.userInterfaceBridge.TextViewNavigator);

            this.shutdownController = new ShutdownController(
                idleMonitor, this.controlsModule.SystemControl, this.application, this.allowShutdown, this.reporters?.ShutdownControllerReporter);

            var menuRequester = new MenuRequester();
            this.playerController = new PlayerController(
                interactionController,
                this.playerModule.PlayerService,
                this.musicPlayer,
                menuRequester,
                this.reporters?.PlayerControllerReporter);

            var timerFactory = new TimerFactory();
            this.ViewNavigator = new ViewNavigator(
                this.volumeModule.VolumeService,
                menuRequester,
                this.shutdownController,
                this.volumeController,
                this.userInterfaceBridge.TextViewNavigator,
                new ViewFactory(this.musicPlayer, this.playerController, this.volumeModule.VolumeService, menuController, this.lifecycleConfiguration),
                timerFactory,
                this.reporters?.ViewNavigatorReporter);

            new DisplayStateController(this.userInterfaceBridge.TextViewNavigator, idleMonitor, this.userInterfaceBridge.Display, this.reporters?.DisplayStateControllerReporter);

            interactionController.Start();
            this.disposer = new Disposer(this.reporters?.DisposableReporter, timerFactory);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.disposer?.Dispose();
        }
    }
}