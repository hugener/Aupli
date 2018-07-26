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
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly ILogger logger;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="logger">The logger.</param>
        public Bootstrapper(IGpioConnectionDriver gpioConnectionDriver, ILogger logger)
        {
            this.gpioConnectionDriver = gpioConnectionDriver;
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
            // Load config
            var configurationRepository = this.CreateConfigurationRepository();
            var configuration = await configurationRepository.GetConfigurationAsync();

            // Create startup: Splash screen etc.
            var startupModule = new StartupModule(
                configuration,
                this.gpioConnectionDriver,
                new TextViewRendererLogger(this.logger),
                new InputManagerLogger(this.logger));
            await startupModule.InitializeAsync();

            // Create required application required systemboundaries modules
            var repositories = this.CreateRepositories();
            await repositories.InitializeAsync();

            // Create domain required application modules
            var lastPlaylistModule = new LastPlaylistModule(repositories.LastPlaylistRepository);

            // Create domain modules
            var playlistModule = new PlaylistModule(lastPlaylistModule.LastPlaylistChangeHandler);

            // Create application required systemboundaries modules
            var controlsModule = new ControlsModule(
                this.gpioConnectionDriver,
                new MusicPlayerLogger(this.logger),
                new AmplifierLogger(this.logger));
            await controlsModule.InitializeAsync();

            // Create application modules
            var playerModule = new PlayerModule(
                repositories.PlaylistRepository,
                playlistModule.LastPlaylistService,
                controlsModule.MusicPlayer,
                new PlayerServiceLogger(this.logger));

            var volumeModule = new VolumeModule(
                repositories.VolumeRepository,
                new Percentage(0.05),
                controlsModule.MusicPlayer,
                controlsModule.MusicPlayer,
                controlsModule.Amplifier,
                new VolumeServiceLogger(this.logger));

            // Create user interface modules
            var userInterfaceModule = new UserInterfaceModule(
                controlsModule,
                playerModule,
                volumeModule,
                startupModule,
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
                startupModule,
                new DisposeAction(async () => await repositories.PlaylistRepository.SaveAsync()));

            // Initialization
            await playlistModule.InitializeAsync();
            await userInterfaceModule.InitializeAsync();
            await userInterfaceModule.ViewNavigator.NavigateToPlayerViewAsync();
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