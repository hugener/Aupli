// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControllerFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.Display;
    using Aupli.Input;
    using Aupli.Lifecycle;
    using Aupli.Logging.Pi.ApplicationFramework.Input;
    using Aupli.Menu;
    using Aupli.Player;
    using Aupli.Volume;
    using Serilog;
    using Sundew.Base.Threading;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Factory for creating various controllers.
    /// </summary>
    public class ControllerFactory : IDisposable
    {
        private readonly Lazy<StartUpController> startUpController;

        private readonly Lazy<DisplayController> displayController;

        private readonly AsyncLazy<IdleController> idleController;

        private readonly AsyncLazy<PlayerController> playerController;

        private readonly Lazy<MenuController> menuController;

        private readonly AsyncLazy<VolumeController> volumeController;

        private readonly AsyncLazy<ViewNavigator> viewNavigator;

        private readonly AsyncLazy<AupliController> aupliController;

        private readonly AsyncLazy<ShutdownController> shutdownController;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerFactory" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="configurationFactory">The configuration factory.</param>
        /// <param name="viewRendererFactory">The textView renderer factory.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <param name="logger">The logger.</param>
        public ControllerFactory(
            ConnectionFactory connectionFactory,
            ConfigurationFactory configurationFactory,
            ViewRendererFactory viewRendererFactory,
            bool allowShutdown,
            CancellationTokenSource cancellationTokenSource,
            ILogger logger)
        {
            this.startUpController = new Lazy<StartUpController>(() =>
                new StartUpController(connectionFactory, viewRendererFactory.TextViewRenderer));

            this.displayController =
                new Lazy<DisplayController>(() => new DisplayController(connectionFactory.Lcd.Device));

            var inputManager = new InputManager(new InputManagerLogger(logger));
            var interactionController = new Lazy<InteractionController>(() => new InteractionController(
                connectionFactory.InputControls,
                inputManager,
                logger));

            this.playerController = new AsyncLazy<PlayerController>(
                async () =>
                    new PlayerController(
                        interactionController.Value,
                        connectionFactory.MusicPlayer,
                        await configurationFactory.GetPlaylistMapAsync(),
                        await configurationFactory.GetSettingsAsync(),
                        logger),
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.idleController = new AsyncLazy<IdleController>(
                async () =>
                {
                    var settings = await configurationFactory.GetSettingsAsync();
                    return new IdleController(
                        interactionController.Value,
                        new SystemActivityAggregator(connectionFactory.MusicPlayer, logger),
                        settings.IdleTimeout,
                        settings.SystemTimeout,
                        new IdleControllerLogger(logger));
                },
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.menuController = new Lazy<MenuController>(() => new MenuController(interactionController.Value));

            this.volumeController = new AsyncLazy<VolumeController>(
                async () => new VolumeController(
                    connectionFactory.MusicPlayer,
                    connectionFactory.MusicPlayer,
                    interactionController.Value,
                    connectionFactory.VolumeControls,
                    await configurationFactory.GetSettingsAsync(),
                    logger),
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.viewNavigator = new AsyncLazy<ViewNavigator>(
                async () =>
                {
                    var screenFactory = new ViewFactory(
                        this,
                        connectionFactory,
                        await configurationFactory.GetSettingsAsync());
                    return new ViewNavigator(viewRendererFactory.TextViewRenderer, screenFactory, logger);
                },
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.aupliController = new AsyncLazy<AupliController>(
                async () => new AupliController(inputManager, await this.viewNavigator, this, logger),
                LazyThreadSafetyMode.ExecutionAndPublication);

            this.shutdownController = new AsyncLazy<ShutdownController>(
                async () => new ShutdownController(
                    await this.idleController,
                    connectionFactory.RemotePi,
                    allowShutdown,
                    cancellationTokenSource,
                    logger), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Gets the start up controller.
        /// </summary>
        /// <value>
        /// The start up controller.
        /// </value>
        public StartUpController StartUpController => this.startUpController.Value;

        /// <summary>
        /// Gets the display controller.
        /// </summary>
        /// <value>
        /// The display controller.
        /// </value>
        public DisplayController DisplayController => this.displayController.Value;

        /// <summary>
        /// Gets the menu controller.
        /// </summary>
        /// <value>
        /// The menu controller.
        /// </value>
        public MenuController MenuController => this.menuController.Value;

        /// <summary>
        /// Gets the volume controller.
        /// </summary>
        /// <value>
        /// The volume controller.
        /// </value>
        public async Task<VolumeController> GetVolumeControllerAsync()
        {
            return await this.volumeController;
        }

        /// <summary>
        /// Gets the shutdown controller.
        /// </summary>
        /// <value>
        /// The shutdown controller.
        /// </value>
        public async Task<ShutdownController> GetShutdownControllerAsync()
        {
            return await this.shutdownController;
        }

        /// <summary>
        /// Gets the idle controller.
        /// </summary>
        /// <value>
        /// The idle controller.
        /// </value>
        public async Task<IdleController> GetIdleControllerAsync()
        {
            return await this.idleController;
        }

        /// <summary>
        /// Gets the player controller asynchronous.
        /// </summary>
        /// <returns>The get player controller task.</returns>
        public async Task<PlayerController> GetPlayerControllerAsync()
        {
            return await this.playerController;
        }

        /// <summary>
        /// Gets the aupli controller asynchronous.
        /// </summary>
        /// <returns>The get aupli controller task.</returns>
        public async Task<AupliController> GetAupliControllerAsync()
        {
            return await this.aupliController;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.idleController.GetAwaiter().GetResult().Dispose();
            this.viewNavigator.GetAwaiter().GetResult().Dispose();
        }
    }
}