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
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Shared.Lifecycle;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using Aupli.SystemBoundaries.UserInterface.StartUp;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Base.Initialization;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Navigation;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Represents the startup module.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public class StartupModule : IInitializable, IInputParameters, IDisposable
    {
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly IGpioConnectionDriverFactory gpioConnectionDriverFactory;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly IInputManagerReporter inputManagerReporter;
        private ITextViewRenderer textViewRenderer;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupModule" /> class.
        /// </summary>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        /// <param name="inputManagerReporter">The input manager reporter.</param>
        public StartupModule(
            ILifecycleConfiguration lifecycleConfiguration,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null)
        {
            this.lifecycleConfiguration = lifecycleConfiguration;
            this.gpioConnectionDriverFactory = gpioConnectionDriverFactory;
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
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            // Create display
            var displayFactory = this.CreateDisplayFactory();
            this.Display = displayFactory.Create(this.gpioConnectionDriverFactory);

            // Create Text Rendering
            var textViewRendererFactory = new TextViewRendererFactory(this.Display, this.textViewRendererReporter);
            this.textViewRenderer = textViewRendererFactory.Create();

            // Show welcome/loading message
            this.InputManager = new InputManager(this.inputManagerReporter);
            this.TextViewNavigator = new TextViewNavigator(this.textViewRenderer, this.InputManager);

            await this.TextViewNavigator.ShowAsync(new StartUpTextView(this.lifecycleConfiguration)).ConfigureAwait(false);
            this.textViewRenderer.Start();
            this.disposer = new Disposer(this.textViewRenderer, displayFactory);
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
    }
}