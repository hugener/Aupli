// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Aupli.IntegrationTests.Bootstrapping
{
    using System;
    using SystemBoundaries.Api;
    using SystemBoundaries.Bridges.Controls;
    using SystemBoundaries.MusicControl;
    using SystemBoundaries.Persistence.Api;
    using SystemBoundaries.Persistence.Configuration.Api;
    using global::NSubstitute;
    using NSubstitute;
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

        public IStartupModule StartupModule { get; private set; }

        public IRepositoriesModule RepositoriesModule { get; private set; }

        public TestControlsModule ControlsModule { get; private set; }

        public TestMusicControlModule MusicControlModule { get; private set; }

        protected override IGpioConnectionDriverFactory CreateGpioConnectionDriverFactory()
        {
            this.GpioConnectionDriverFactory = Substitute.For<IGpioConnectionDriverFactory>();
            return this.GpioConnectionDriverFactory;
        }

        protected override IStartupModule CreateStartupModule(IApplication application, IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            this.StartupModule = new TestStartupModule(application, gpioConnectionDriverFactory);
            return this.StartupModule;
        }

        protected override IRepositoriesModule CreateRepositoriesModule()
        {
            this.RepositoriesModule = Substitute.For<IRepositoriesModule>();
            var configurationRepository = this.RepositoriesModule.ConfigurationRepository.ReturnsSubstitute();
            configurationRepository.GetConfigurationAsync()
                .Returns(new Configuration(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(4)));
            this.RepositoriesModule.LastPlaylistRepository.ReturnsSubstitute();
            this.RepositoriesModule.PlaylistRepository.ReturnsSubstitute();
            this.RepositoriesModule.VolumeRepository.ReturnsSubstitute();
            return this.RepositoriesModule;
        }

        protected override IControlsModule CreateControlsModule(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            this.ControlsModule = new TestControlsModule(gpioConnectionDriverFactory, null);
            return this.ControlsModule;
        }

        protected override MusicControlModule CreateMusicControlModule()
        {
            this.MusicControlModule = new TestMusicControlModule(null);
            return this.MusicControlModule;
        }
    }
}