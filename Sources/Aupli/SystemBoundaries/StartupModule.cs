// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupModule.cs" company="Hukano">
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
    using Aupli.SystemBoundaries.SystemServices;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Unix;
    using Aupli.SystemBoundaries.UserInterface.Startup;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Navigation;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Represents the startup module.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class StartupModule : IStartupModule
    {
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly string namePath;
        private readonly string pin26FeaturePath;
        private readonly string greetingsPath;
        private readonly string lastGreetingPath;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly IInputManagerReporter inputManagerReporter;
        private ITextViewRenderer textViewRenderer;
        private Disposer disposer;
        private ISystemServicesAwaiter systemServicesAwaiter;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupModule" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="namePath">The name path.</param>
        /// <param name="pin26FeaturePath">The pin26 feature path.</param>
        /// <param name="greetingsPath">The greetings path.</param>
        /// <param name="lastGreetingPath">The last greeting path.</param>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        /// <param name="inputManagerReporter">The input manager reporter.</param>
        public StartupModule(
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            string namePath = "name.val",
            string pin26FeaturePath = "pin26-feature.val",
            string greetingsPath = "greetings.csv",
            string lastGreetingPath = "last-greeting.val",
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null)
        {
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.namePath = namePath;
            this.pin26FeaturePath = pin26FeaturePath;
            this.greetingsPath = greetingsPath;
            this.lastGreetingPath = lastGreetingPath;
            this.textViewRendererReporter = textViewRendererReporter;
            this.inputManagerReporter = inputManagerReporter;
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public IDisplay Display { get; private set; }

        /// <summary>
        /// Gets the text view navigator.
        /// </summary>
        /// <value>
        /// The text view navigator.
        /// </value>
        public TextViewNavigator TextViewNavigator { get; private set; }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input manager.
        /// </value>
        public InputManager InputManager { get; private set; }

        /// <summary>
        /// Gets the lifecycle configuration.
        /// </summary>
        /// <value>
        /// The lifecycle configuration.
        /// </value>
        public ILifecycleConfiguration LifecycleConfiguration { get; private set; }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            this.LifecycleConfiguration = await this.GetLifecycleConfigurationAsync();
            var greetingProvider = this.CreateGreetingProvider();

            // Create display
            var displayFactory = this.CreateDisplayFactory();
            this.Display = displayFactory.Create(this.gpioConnectionDriverFactory, this.LifecycleConfiguration.Pin26Feature == Pin26Feature.Backlight);

            // Create Text Rendering
            var textViewRendererFactory = new TextViewRendererFactory(this.Display, this.textViewRendererReporter);
            this.textViewRenderer = textViewRendererFactory.Create();

            // Show welcome/loading message
            this.InputManager = new InputManager(this.inputManagerReporter);
            this.TextViewNavigator = new TextViewNavigator(this.textViewRenderer, this.InputManager);

            await this.TextViewNavigator.ShowAsync(new StartupTextView(greetingProvider, this.LifecycleConfiguration)).ConfigureAwait(false);
            this.textViewRenderer.Start();

            this.systemServicesAwaiter = this.CreateServicesAwaiter();
            this.disposer = new Disposer(this.textViewRenderer, displayFactory);
        }

        /// <summary>
        /// Waits for dependencies.
        /// </summary>
        public Task<bool> WaitForSystemServicesAsync()
        {
            return this.systemServicesAwaiter.WaitForServicesAsync(new[] { "mpd" }, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.disposer?.Dispose();
            this.disposer = null;
        }

        /// <summary>
        /// Creates the display factory.
        /// </summary>
        /// <returns>The display factory.</returns>
        protected virtual DisplayFactory CreateDisplayFactory()
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

        /// <summary>
        /// Creates the services awaiter.
        /// </summary>
        /// <returns>A <see cref="UnixSystemServiceStateChecker"/>.</returns>
        protected virtual ISystemServicesAwaiter CreateServicesAwaiter()
        {
            return new SystemServicesAwaiter();
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
    }
}