// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Bootstrapping
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices;
    using Aupli.DomainServices;
    using Aupli.Logging.Serilog.ApplicationServices.Player;
    using Aupli.Logging.Serilog.ApplicationServices.Volume;
    using Aupli.Logging.Serilog.Mpc;
    using Aupli.Logging.Serilog.Pi.ApplicationFramework.Input;
    using Aupli.Logging.Serilog.Pi.ApplicationFramework.ViewRendering;
    using Aupli.Logging.Serilog.SystemBoundaries.Pi.Amplifier;
    using Aupli.Logging.Serilog.SystemBoundaries.Pi.Display;
    using Aupli.Logging.Serilog.SystemBoundaries.Pi.Interaction;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Input;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Player;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Volume;
    using Aupli.SystemBoundaries;
    using Aupli.SystemBoundaries.Persistence.Configuration;
    using Aupli.SystemBoundaries.Persistence.Playlists;
    using Aupli.SystemBoundaries.Persistence.Volume;
    using Aupli.SystemBoundaries.UserInterface;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Sundew.Base.Disposal;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Bootstraps the application.
    /// </summary>
    public class Bootstrapper
    {
        private readonly ILogger logger;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Bootstrapper(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Starts the asynchronous.
        /// </summary>
        /// <param name="shutdownCancellationTokenSource">The shutdown cancellation token source.</param>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task StartAsync(CancellationTokenSource shutdownCancellationTokenSource, bool allowShutdown)
        {
            var configurationTask = Task.Run(async () =>
            {
                var configurationRepository2 = this.CreateConfigurationRepository();
                var configuration2 = await configurationRepository2.GetConfigurationAsync().ConfigureAwait(false);
                return (configurationRepository2, configuration2);
            }).ConfigureAwait(false);

            this.logger.Verbose("Create GpioConnectionDriverFactory");
            var gpioConnectionDriverFactory = new GpioConnectionDriverFactory(true);

            var (configurationRepository, configuration) = await configurationTask;

            // Create startup: Splash screen etc.
            var startupModuleTask = Task.Run(async () =>
            {
                this.logger.Verbose("Create Startup Module");
                var startupModule = new StartupModule(
                    configuration,
                    gpioConnectionDriverFactory,
                    new TextViewRendererLogger(this.logger),
                    new InputManagerLogger(this.logger));
                this.logger.Verbose("Initialize Startup Module");
                await startupModule.InitializeAsync();
                return startupModule;
            }).ConfigureAwait(false);

            this.logger.Verbose("Create Repositories");

            // Create required application required systemboundaries modules
            var repositories = this.CreateRepositories();
            this.logger.Verbose("Initialize Repositories");
            await repositories.InitializeAsync();

            // Create domain required application modules
            this.logger.Verbose("Create LastPlaylistModule");
            var lastPlaylistModule = new LastPlaylistModule(repositories.LastPlaylistRepository);

            // Create domain modules
            this.logger.Verbose("Create PlaylistModule");
            var playlistModule = new PlaylistModule(lastPlaylistModule.LastPlaylistChangeHandler);

            // Create application required systemboundaries modules
            this.logger.Verbose("Create ControlsModule");
            var controlsModule = new ControlsModule(
                gpioConnectionDriverFactory,
                new MusicPlayerLogger(this.logger),
                new AmplifierLogger(this.logger));
            await controlsModule.InitializeAsync();

            // Create application modules
            this.logger.Verbose("Create PlayerModule");
            var playerModule = new PlayerModule(
                repositories.PlaylistRepository,
                playlistModule.LastPlaylistService,
                controlsModule.MusicPlayer,
                new PlayerServiceLogger(this.logger));

            this.logger.Verbose("Create Volume Module");
            var volumeModule = new VolumeModule(
                repositories.VolumeRepository,
                new Percentage(0.05),
                controlsModule.MusicPlayer,
                controlsModule.MusicPlayer,
                controlsModule.Amplifier,
                controlsModule.MusicPlayer,
                new VolumeServiceLogger(this.logger));

            await volumeModule.InitializeAsync();

            // Create user interface modules
            this.logger.Verbose("Create UserInterface Module");
            var userInterfaceModule = new UserInterfaceModule(
                controlsModule,
                playerModule,
                volumeModule,
                await startupModuleTask,
                new ShutdownParameters(allowShutdown, shutdownCancellationTokenSource),
                configuration,
                configuration,
                new Reporters(
                    new InteractionControllerLogger(this.logger),
                    new SystemActivityAggregatorLogger(this.logger),
                    new IdleControllerLogger(this.logger),
                    new PlayerControllerLogger(this.logger),
                    new VolumeControllerLogger(this.logger),
                    new ShutdownControllerLogger(this.logger),
                    new ViewNavigatorLogger(this.logger),
                    new DisplayBacklightControllerLogger(this.logger)));

            this.disposer = new Disposer(
                userInterfaceModule,
                controlsModule,
                await startupModuleTask,
                gpioConnectionDriverFactory,
                new DisposeAction(async () => await repositories.PlaylistRepository.SaveAsync()));

            this.logger.Verbose("Initialize Playlist Module");
            await playlistModule.InitializeAsync();
            this.logger.Verbose("Initialize UserInterface Module");
            await userInterfaceModule.InitializeAsync();
            await userInterfaceModule.ViewNavigator.NavigateToPlayerViewAsync();
            this.logger.Verbose("Save config");
            await configurationRepository.SaveConfigurationAsync();
        }

        /// <summary>
        /// Stops the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public Task StopAsync()
        {
            this.disposer.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates the configuration repository.
        /// </summary>
        /// <returns>The configuration repository.</returns>
        protected virtual IConfigurationRepository CreateConfigurationRepository()
        {
            return new ConfigurationJsonFileRepository("configuration.json");
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <returns>The repositories.</returns>
        protected virtual Repositories CreateRepositories()
        {
            return new Repositories(
                new VolumeJsonFileRepository("volume.json"),
                new PlaylistMapJsonFileRepository("playlists.json"),
                new LastPlaylistJsonFileRepository("last-playlist.json"));
        }
    }
}