// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Api;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Aupli.SystemBoundaries.Persistence.Startup;
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Display.Api;
    using Aupli.SystemBoundaries.SystemServices;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using Aupli.SystemBoundaries.SystemServices.Unix;
    using Aupli.SystemBoundaries.UserInterface.Startup;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.TextView.ApplicationFramework;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.Navigation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Represents the startup module.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class StartupModule : IStartupModule
    {
        private readonly IApplicationRendering application;
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly string namePath;
        private readonly string pin26FeaturePath;
        private readonly string greetingsPath;
        private readonly string lastGreetingPath;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly IInputManagerReporter inputManagerReporter;
        private readonly ISystemServicesAwaiterReporter systemServicesAwaiterReporter;
        private Disposer disposer;
        private ISystemServicesAwaiter systemServicesAwaiter;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupModule" /> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="namePath">The name path.</param>
        /// <param name="pin26FeaturePath">The pin26 feature path.</param>
        /// <param name="greetingsPath">The greetings path.</param>
        /// <param name="lastGreetingPath">The last greeting path.</param>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        /// <param name="inputManagerReporter">The input manager reporter.</param>
        /// <param name="systemServicesAwaiterReporter">The system services awaiter reporter.</param>
        public StartupModule(
            IApplicationRendering application,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            string namePath = "name.val",
            string pin26FeaturePath = "pin26-feature.val",
            string greetingsPath = "greetings.csv",
            string lastGreetingPath = "last-greeting.val",
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null,
            ISystemServicesAwaiterReporter systemServicesAwaiterReporter = null)
        {
            this.application = application;
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
            this.namePath = namePath;
            this.pin26FeaturePath = pin26FeaturePath;
            this.greetingsPath = greetingsPath;
            this.lastGreetingPath = lastGreetingPath;
            this.textViewRendererReporter = textViewRendererReporter;
            this.inputManagerReporter = inputManagerReporter;
            this.systemServicesAwaiterReporter = systemServicesAwaiterReporter;
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
        public ITextViewNavigator TextViewNavigator { get; private set; }

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

            this.application.InputManagerReporter = this.inputManagerReporter;
            this.application.TextViewRendererReporter = this.textViewRendererReporter;

            this.TextViewNavigator = this.application.StartRendering(this.Display);
            await this.TextViewNavigator.ShowAsync(new StartupTextView(greetingProvider, this.LifecycleConfiguration)).ConfigureAwait(false);

            this.systemServicesAwaiter = this.CreateServicesAwaiter();
            this.disposer = new Disposer(displayFactory);
        }

        /// <summary>
        /// Waits for dependencies.
        /// </summary>
        public Task<bool> WaitForSystemServicesAsync()
        {
            return this.systemServicesAwaiter.WaitForServicesAsync(new[] { "mpd" }, Timeout.InfiniteTimeSpan);
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

        /// <summary>
        /// Creates the services awaiter.
        /// </summary>
        /// <returns>A <see cref="UnixSystemServiceStateChecker"/>.</returns>
        protected virtual ISystemServicesAwaiter CreateServicesAwaiter()
        {
            return new SystemServicesAwaiter(this.systemServicesAwaiterReporter);
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