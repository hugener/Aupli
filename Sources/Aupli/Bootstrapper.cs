// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System.Threading.Tasks;
    using Aupli.ApplicationServices;
    using Aupli.DomainServices;
    using Aupli.Logging.Serilog.ApplicationServices.Player;
    using Aupli.Logging.Serilog.ApplicationServices.Volume;
    using Aupli.Logging.Serilog.SundewBase;
    using Aupli.Logging.Serilog.SystemBoundaries.MusicControl;
    using Aupli.Logging.Serilog.SystemBoundaries.Pi.Amplifier;
    using Aupli.Logging.Serilog.SystemBoundaries.SystemServices;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Display;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Input;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Player;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Volume;
    using Aupli.Logging.Serilog.TextView.ApplicationFramework.Input;
    using Aupli.Logging.Serilog.TextView.ApplicationFramework.ViewRendering;
    using Aupli.SystemBoundaries;
    using Aupli.SystemBoundaries.Api;
    using Aupli.SystemBoundaries.Bridges.Controls;
    using Aupli.SystemBoundaries.MusicControl;
    using Aupli.SystemBoundaries.Persistence;
    using Aupli.SystemBoundaries.Persistence.Api;
    using Aupli.SystemBoundaries.Pi;
    using Aupli.SystemBoundaries.SystemServices;
    using Aupli.SystemBoundaries.UserInterface;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Serilog.Events;
    using Sundew.Base.Disposal;
    using Sundew.Base.Numeric;
    using Sundew.TextView.ApplicationFramework;

    /// <summary>
    /// Bootstraps the application.
    /// </summary>
    public class Bootstrapper
    {
        private readonly IApplication application;
        private readonly ILogger logger;
        private readonly DisposableLogger disposableLogger;
        private Disposer? disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="logger">The logger.</param>
        public Bootstrapper(IApplication application, ILogger logger)
        {
            this.application = application;
            this.logger = logger.ForContext<Bootstrapper>();
            this.disposableLogger = new DisposableLogger(this.logger);
        }

        /// <summary>
        /// Starts the asynchronous.
        /// </summary>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task StartAsync(bool allowShutdown)
        {
            this.logger.Verbose("Create GpioConnectionDriverFactory");
            var gpioConnectionDriverFactory = this.CreateGpioConnectionDriverFactory();

            // Create Startup Module
            this.logger.Verbose("Create Startup module");
            var startupModuleFactory = this.CreateStartupModule(this.application, gpioConnectionDriverFactory);
            var startupModule = await startupModuleFactory.StartupModule.ConfigureAwait(false);
            this.logger.Verbose("Navigate to Startup view");
            await startupModule.NavigateToStartupViewAsync().ConfigureAwait(false);

            // Create required ApplicationServices-required system boundaries modules
            this.logger.Verbose("Create Repositories Module");
            var repositoriesModule = this.CreateRepositoriesModule();
            this.logger.Verbose("Initialize Repositories Module");
            var repositoriesModuleTask = repositoriesModule.InitializeAsync().ConfigureAwait(false);

            // Create domain required application modules
            this.logger.Verbose("Create LastPlaylist Module");
            var lastPlaylistModule = new LastPlaylistModule(repositoriesModule.LastPlaylistRepository);

            // Create domain modules
            this.logger.Verbose("Create Playlist Module");
            var playlistModule = new PlaylistModule(lastPlaylistModule.LastPlaylistChangeHandler);

            // Create application required system boundaries modules
            this.logger.Verbose("Create Controls Module");
            var controlsModuleFactory = this.CreateControlsModule(gpioConnectionDriverFactory);
            var controlsModule = await controlsModuleFactory.ControlsModule;

            // Create music control module.
            var musicControlModule = this.CreateMusicControlModule();
            await musicControlModule.InitializeAsync().ConfigureAwait(false);

            this.logger.Verbose("Create System services");
            var systemServicesModule = this.CreateSystemServicesModule();
            await systemServicesModule.InitializeAsync().ConfigureAwait(false);

            // Create application modules
            this.logger.Verbose("Create Player Module");
            var playerModule = this.CreatePlayerModule(repositoriesModule, playlistModule, musicControlModule);
            await playerModule.InitializeAsync().ConfigureAwait(false);

            this.logger.Verbose("Create Volume Module");
            var volumeModule = new VolumeModule(
                controlsModule.Amplifier,
                musicControlModule.MusicPlayer,
                musicControlModule.MusicPlayer,
                repositoriesModule.VolumeRepository,
                new Percentage(0.05),
                new VolumeServiceLogger(this.logger));
            await repositoriesModuleTask;
            await volumeModule.InitializeAsync().ConfigureAwait(false);

            // Create user interface modules
            this.logger.Verbose("Create UserInterface Module");
            var userInterfaceModule = new UserInterfaceModule(
                this.application,
                startupModule,
                controlsModule,
                musicControlModule.MusicPlayer,
                playerModule,
                volumeModule,
                allowShutdown,
                startupModule.LifecycleConfiguration,
                await repositoriesModule.ConfigurationRepository.GetConfigurationAsync().ConfigureAwait(false),
                new Reporters(
                    new InteractionControllerLogger(this.logger),
                    new SystemActivityAggregatorLogger(this.logger),
                    new IdleMonitorLogger(this.logger),
                    new PlayerControllerLogger(this.logger),
                    new VolumeControllerLogger(this.logger),
                    new ShutdownControllerLogger(this.logger),
                    new ViewNavigatorLogger(this.logger),
                    new DisplayStateControllerLogger(this.logger),
                    this.disposableLogger));

            this.disposer = new Disposer(
                this.disposableLogger,
                new DisposeAction(async () => await repositoriesModule.PlaylistRepository.SaveAsync().ConfigureAwait(false)),
                userInterfaceModule,
                musicControlModule,
                controlsModuleFactory,
                startupModuleFactory,
                gpioConnectionDriverFactory);

            this.logger.Verbose("Initialize Playlist Module");
            await playlistModule.InitializeAsync().ConfigureAwait(false);
            this.logger.Verbose("Initialize UserInterface Module");
            await userInterfaceModule.InitializeAsync().ConfigureAwait(false);
            await userInterfaceModule.ViewNavigator.NavigateToPlayerViewAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public Task StopAsync()
        {
            this.disposer?.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates the startup module.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>
        /// The startup module.
        /// </returns>
        protected virtual IStartupModuleFactory CreateStartupModule(IApplication application, IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new StartupModuleFactory(
                application,
                gpioConnectionDriverFactory,
                "name.val",
                "pin26-feature.val",
                "greetings.csv",
                "last-greeting.val",
                new TextViewRendererLogger(this.logger),
                new InputManagerLogger(this.logger),
                this.disposableLogger,
                new TimeIntervalSynchronizerLogger(this.logger));
        }

        /// <summary>
        /// Creates the gpio connection driver factory.
        /// </summary>
        /// <returns>A <see cref="GpioConnectionDriverFactory"/>.</returns>
        protected virtual IGpioConnectionDriverFactory CreateGpioConnectionDriverFactory()
        {
            return new GpioConnectionDriverFactory(true);
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <returns>The repositories.</returns>
        protected virtual IRepositoriesModule CreateRepositoriesModule()
        {
            return new RepositoriesModule("volume.val", "playlists.json", "last-playlist.json", "configuration.json");
        }

        /// <summary>
        /// Creates the controls module.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>A <see cref="ControlsModuleFactory"/>.</returns>
        protected virtual IControlsModuleFactory CreateControlsModule(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new ControlsModuleFactory(
                gpioConnectionDriverFactory,
                new AmplifierLogger(this.logger, LogEventLevel.Debug),
                this.disposableLogger);
        }

        /// <summary>
        /// Creates the music control module.
        /// </summary>
        /// <returns>A <see cref="MusicControlModule"/>.</returns>
        protected virtual MusicControlModule CreateMusicControlModule()
        {
            return new MusicControlModule(new MusicPlayerLogger(this.logger));
        }

        /// <summary>Creates the system services module.</summary>
        /// <returns>A <see cref="SystemServicesModule"/>.</returns>
        protected virtual SystemServicesModule CreateSystemServicesModule()
        {
            return new SystemServicesModule(
                new SystemServicesAwaiterLogger(this.logger),
                new WifiConnecterLogger(this.logger));
        }

        /// <summary>
        /// Creates the player module.
        /// </summary>
        /// <param name="repositoriesModule">The repositories module.</param>
        /// <param name="playlistModule">The playlist module.</param>
        /// <param name="musicControlModule">The music control module.</param>
        /// <returns>A <see cref="PlayerModule"/>.</returns>
        protected virtual PlayerModule CreatePlayerModule(IRepositoriesModule repositoriesModule, PlaylistModule playlistModule, MusicControlModule musicControlModule)
        {
            return new PlayerModule(
                repositoriesModule.PlaylistRepository,
                playlistModule.LastPlaylistService,
                musicControlModule.MusicPlayer,
                new PlayerServiceLogger(this.logger));
        }
    }
}