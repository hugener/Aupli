// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries
{
    using System;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Api;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Aupli.SystemBoundaries.Persistence.Startup;
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Display.Api;
    using Aupli.SystemBoundaries.UserInterface.Startup;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Base.Threading;
    using Sundew.TextView.ApplicationFramework;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.Navigation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Represents the startup module.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Api.IStartupModuleFactory" />
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class StartupModuleFactory : IStartupModuleFactory
    {
        private readonly IApplicationRendering application;
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly string namePath;
        private readonly string pin26FeaturePath;
        private readonly string greetingsPath;
        private readonly string lastGreetingPath;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly IInputManagerReporter inputManagerReporter;
        private readonly AsyncLazy<IStartupModule, StartupModuleData> startupModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemBoundaries.StartupModuleFactory" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="namePath">The name path.</param>
        /// <param name="pin26FeaturePath">The pin26 feature path.</param>
        /// <param name="greetingsPath">The greetings path.</param>
        /// <param name="lastGreetingPath">The last greeting path.</param>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        /// <param name="inputManagerReporter">The input manager reporter.</param>
        public StartupModuleFactory(
            IApplicationRendering application,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            string namePath = "name.val",
            string pin26FeaturePath = "pin26-feature.val",
            string greetingsPath = "greetings.csv",
            string lastGreetingPath = "last-greeting.val",
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null)
        {
            this.application = application;
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.namePath = namePath;
            this.pin26FeaturePath = pin26FeaturePath;
            this.greetingsPath = greetingsPath;
            this.lastGreetingPath = lastGreetingPath;
            this.textViewRendererReporter = textViewRendererReporter;
            this.inputManagerReporter = inputManagerReporter;
            this.startupModule = new AsyncLazy<IStartupModule, StartupModuleData>(
                async () =>
                {
                    var lifecycleConfiguration = await this.GetLifecycleConfigurationAsync();
                    var greetingProvider = this.CreateGreetingProvider();

                    var displayFactory = this.CreateDisplayFactory();
                    var display = displayFactory.Create(
                        this.gpioConnectionDriverFactory,
                        lifecycleConfiguration.Pin26Feature == Pin26Feature.Backlight);

                    this.application.InputManagerReporter = this.inputManagerReporter;
                    this.application.TextViewRendererReporter = this.textViewRendererReporter;

                    var textViewNavigator = this.application.StartRendering(display);
                    var disposer = new Disposer(displayFactory);
                    return new StartupModuleData(display, textViewNavigator, lifecycleConfiguration, greetingProvider, disposer);
                });
        }

        /// <summary>
        /// Gets the <see cref="IStartupModule"/>.
        /// </summary>
        /// <returns>A task with the startup data.</returns>
        public IAsyncLazy<IStartupModule> StartupModule => this.startupModule;

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            var values = await this.startupModule;
            await values.TextViewNavigator.ShowAsync(new StartupTextView(values.GreetingProvider, values.LifecycleConfiguration)).ConfigureAwait(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.startupModule.GetValueOrDefault()?.Disposer.Dispose();
        }

        /// <summary>
        /// Creates the display factory.
        /// </summary>
        /// <returns>The display factory.</returns>
        protected virtual IDisplayFactory CreateDisplayFactory()
        {
            return new DisplayFactory();
        }

        /// <summary>
        /// Creates the greeting provider.
        /// </summary>
        /// <returns>The greeting provider.</returns>
        protected virtual IGreetingProvider CreateGreetingProvider()
        {
            return new GreetingTextFileRepository(this.greetingsPath, this.lastGreetingPath);
        }

        /// <summary>
        /// Gets the lifecycle configuration asynchronous.
        /// </summary>
        /// <returns>The lifecycle configuration.</returns>
        protected virtual async Task<ILifecycleConfiguration> GetLifecycleConfigurationAsync()
        {
            var nameTextFileRepository = new NameTextFileRepository(this.namePath);
            var pin26FeatureTextFileRepository = new Pin26FeatureTextFileRepository(this.pin26FeaturePath);
            return new PrivateLifecycleConfiguration(await nameTextFileRepository.GetNameAsync(), await pin26FeatureTextFileRepository.GetPin26FeatureAsync());
        }

        private class PrivateLifecycleConfiguration : ILifecycleConfiguration
        {
            public PrivateLifecycleConfiguration(string name, Pin26Feature pin26Feature)
            {
                this.Name = name;
                this.Pin26Feature = pin26Feature;
            }

            public string Name { get; }

            public Pin26Feature Pin26Feature { get; }
        }

        private class StartupModuleData : IStartupModule
        {
            public StartupModuleData(
                IDisplay display,
                ITextViewNavigator textViewNavigator,
                ILifecycleConfiguration lifecycleConfiguration,
                IGreetingProvider greetingProvider,
                IDisposable disposer)
            {
                this.Display = display;
                this.TextViewNavigator = textViewNavigator;
                this.LifecycleConfiguration = lifecycleConfiguration;
                this.GreetingProvider = greetingProvider;
                this.Disposer = disposer;
            }

            public IDisplay Display { get; }

            public ITextViewNavigator TextViewNavigator { get; }

            public ILifecycleConfiguration LifecycleConfiguration { get; }

            public IGreetingProvider GreetingProvider { get; }

            public IDisposable Disposer { get; }
        }
    }
}