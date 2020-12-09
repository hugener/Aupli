// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartupTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Startup
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <inheritdoc />
    /// <summary>
    /// The textView shown at startup.
    /// </summary>
    /// <seealso cref="T:Aupli.ViewRendering.ITextView" />
    public class StartupTextView : ITextView
    {
        private readonly IGreetingProvider greetingProvider;
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private string greeting = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartupTextView" /> class.
        /// </summary>
        /// <param name="greetingProvider">The greeting provider.</param>
        /// <param name="lifecycleConfiguration">The startup configuration.</param>
        public StartupTextView(IGreetingProvider greetingProvider, ILifecycleConfiguration lifecycleConfiguration)
        {
            this.greetingProvider = greetingProvider;
            this.lifecycleConfiguration = lifecycleConfiguration;
        }

        /// <inheritdoc />
        public IEnumerable<object>? InputTargets => null;

        /// <inheritdoc />
        public async Task OnShowingAsync(IInvalidater invalidater, ICharacterContext? characterContext)
        {
            this.greeting = await this.greetingProvider.GetGreetingAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public void OnDraw(IRenderContext renderContext)
        {
            renderContext.Clear();
            renderContext.SetPosition(0, 0);
            renderContext.WriteLine($"{this.greeting} {this.lifecycleConfiguration.Name}");
        }

        /// <inheritdoc />
        public Task OnClosingAsync()
        {
            return this.greetingProvider.SaveLastGreetingAsync(this.greeting);
        }
    }
}