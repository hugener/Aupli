// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices;
    using Aupli.DomainServices;
    using Aupli.SystemBoundaries.Api;
    using Aupli.SystemBoundaries.Bridges.Controls;
    using Aupli.SystemBoundaries.MusicControl;
    using Aupli.SystemBoundaries.Persistence.Api;
    using Aupli.SystemBoundaries.Persistence.Configuration.Api;
    using global::NSubstitute;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Sundew.TextView.ApplicationFramework;

    public class TestBootstrapper : Bootstrapper
    {
        public TestBootstrapper(IApplication application, ILogger logger)
            : base(application, logger)
        {
        }

        public IGpioConnectionDriverFactory GpioConnectionDriverFactory { get; private set; }

        public IStartupModuleFactory StartupModuleFactory { get; private set; }

        public IRepositoriesModule RepositoriesModule { get; private set; }

        public TestControlsModuleFactory ControlsModuleFactory { get; private set; }

        public TestMusicControlModule MusicControlModule { get; private set; }

        protected override IGpioConnectionDriverFactory CreateGpioConnectionDriverFactory()
        {
            this.GpioConnectionDriverFactory = Substitute.For<IGpioConnectionDriverFactory>();
            return this.GpioConnectionDriverFactory;
        }

        protected override IStartupModuleFactory CreateStartupModule(IApplication application, IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            this.StartupModuleFactory = new TestStartupModuleFactory(application, gpioConnectionDriverFactory);
            return this.StartupModuleFactory;
        }

        protected override IRepositoriesModule CreateRepositoriesModule()
        {
            this.RepositoriesModule = Substitute.For<IRepositoriesModule>();
            var configurationRepository = this.RepositoriesModule.ConfigurationRepository;
            configurationRepository.GetConfigurationAsync()
                .Returns(Task.FromResult(new Configuration(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(4))));
            return this.RepositoriesModule;
        }

        protected override IControlsModuleFactory CreateControlsModule(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            this.ControlsModuleFactory = new TestControlsModuleFactory(gpioConnectionDriverFactory, null, null);
            return this.ControlsModuleFactory;
        }

        protected override MusicControlModule CreateMusicControlModule()
        {
            this.MusicControlModule = new TestMusicControlModule(null);
            return this.MusicControlModule;
        }

        protected override PlayerModule CreatePlayerModule(IRepositoriesModule repositoriesModule, PlaylistModule playlistModule, MusicControlModule musicControlModule)
        {
            return new TestPlayerModule(repositoriesModule.PlaylistRepository, playlistModule.LastPlaylistService, musicControlModule.MusicPlayer, null, null, null);
        }
    }
}