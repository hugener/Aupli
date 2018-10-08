// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
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
    using Aupli.SystemBoundaries.Api;
    using Aupli.SystemBoundaries.MusicControl;
    using Aupli.SystemBoundaries.Persistence;
    using Aupli.SystemBoundaries.Persistence.Api;
    using Aupli.SystemBoundaries.Pi;
    using Aupli.SystemBoundaries.UserInterface;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Serilog.Events;
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
            this.logger.Verbose("Create GpioConnectionDriverFactory");
            var gpioConnectionDriverFactory = new GpioConnectionDriverFactory(true);

            // Create Startup Module
            this.logger.Verbose("Create Startup module");
            var startupModule = this.CreateStartupModule(gpioConnectionDriverFactory);
            this.logger.Verbose("Initialize Startup module");
            await startupModule.InitializeAsync();

            // Create required ApplicationServices-required system boundaries modules
            this.logger.Verbose("Create Repositories");
            var repositoriesModule = this.CreateRepositoriesModule();
            this.logger.Verbose("Initialize Repositories Module");
            await repositoriesModule.InitializeAsync();

            // Create domain required application modules
            this.logger.Verbose("Create LastPlaylistModule");
            var lastPlaylistModule = new LastPlaylistModule(repositoriesModule.LastPlaylistRepository);

            // Create domain modules
            this.logger.Verbose("Create PlaylistModule");
            var playlistModule = new PlaylistModule(lastPlaylistModule.LastPlaylistChangeHandler);

            // Wait for services to be ready.
            await startupModule.WaitForSystemServicesAsync();

            // Create application required system boundaries modules
            this.logger.Verbose("Create ControlsModule");
            var controlsModule = new ControlsModule(
                gpioConnectionDriverFactory,
                new AmplifierLogger(this.logger, LogEventLevel.Debug));
            await controlsModule.InitializeAsync().ConfigureAwait(false);

            // Create music control module.
            var musicControlModule = new MusicControlModule(new MusicPlayerLogger(this.logger));
            await musicControlModule.InitializeAsync();

            // Create application modules
            this.logger.Verbose("Create PlayerModule");
            var playerModule = new PlayerModule(
                repositoriesModule.PlaylistRepository,
                playlistModule.LastPlaylistService,
                musicControlModule.MusicPlayer,
                new PlayerServiceLogger(this.logger));

            this.logger.Verbose("Create Volume Module");
            var volumeModule = new VolumeModule(
                repositoriesModule.VolumeRepository,
                new Percentage(0.05),
                musicControlModule.MusicPlayer,
                musicControlModule.MusicPlayer,
                controlsModule.Amplifier,
                musicControlModule.MusicPlayer,
                new VolumeServiceLogger(this.logger));

            await volumeModule.InitializeAsync().ConfigureAwait(false);

            // Create user interface modules
            this.logger.Verbose("Create UserInterface Module");
            var userInterfaceModule = new UserInterfaceModule(
                startupModule,
                controlsModule,
                musicControlModule.MusicPlayer,
                playerModule,
                volumeModule,
                new ShutdownParameters(allowShutdown, shutdownCancellationTokenSource),
                startupModule.LifecycleConfiguration,
                await repositoriesModule.ConfigurationRepository.GetConfigurationAsync(),
                new Reporters(
                    new InteractionControllerLogger(this.logger),
                    new SystemActivityAggregatorLogger(this.logger),
                    new IdleControllerLogger(this.logger),
                    new PlayerControllerLogger(this.logger),
                    new VolumeControllerLogger(this.logger),
                    new ShutdownControllerLogger(this.logger),
                    new ViewNavigatorLogger(this.logger),
                    new DisplayStateControllerLogger(this.logger)));

            this.disposer = new Disposer(
                controlsModule,
                gpioConnectionDriverFactory,
                new DisposeAction(async () => await repositoriesModule.PlaylistRepository.SaveAsync()),
                userInterfaceModule,
                musicControlModule,
                startupModule);

            this.logger.Verbose("Initialize Playlist Module");
            await playlistModule.InitializeAsync().ConfigureAwait(false);
            this.logger.Verbose("Initialize UserInterface Module");
            await userInterfaceModule.InitializeAsync().ConfigureAwait(false);
            await userInterfaceModule.ViewNavigator.NavigateToPlayerViewAsync();
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
        /// Creates the startup module.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>The startup module.</returns>
        protected virtual IStartupModule CreateStartupModule(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            return new StartupModule(
                gpioConnectionDriverFactory,
                "name.val",
                "pin26-feature.val",
                "greetings.csv",
                "last-greeting.val",
                new TextViewRendererLogger(this.logger),
                new InputManagerLogger(this.logger));
        }

        /// <summary>
        /// Gets the repositories.
        /// </summary>
        /// <returns>The repositories.</returns>
        protected virtual IRepositoriesModule CreateRepositoriesModule()
        {
            return new RepositoriesModule("volume.json", "playlists.json", "last-playlist.json", "configuration.json");
        }
    }
}