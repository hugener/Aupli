// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartUpTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.StartUp
{
    using System.Collections.Generic;
    using Aupli.SystemBoundaries.Connectors.Lifecycle;
    using Sundew.Base.Collections;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <inheritdoc />
    /// <summary>
    /// The textView shown at startup.
    /// </summary>
    /// <seealso cref="T:Aupli.ViewRendering.ITextView" />
    public class StartUpTextView : ITextView
    {
        private static readonly string[] Greetings = { "Hi", "Hej", "Hola", "Hoi" };
        private readonly ILifecycleConfiguration lifecycleConfiguration;
        private readonly string greeting;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpTextView" /> class.
        /// </summary>
        /// <param name="lifecycleConfiguration">The startup configuration.</param>
        public StartUpTextView(ILifecycleConfiguration lifecycleConfiguration)
        {
            this.lifecycleConfiguration = lifecycleConfiguration;
            var lastGreeting = string.IsNullOrEmpty(this.lifecycleConfiguration.LastGreeting)
                ? Greetings[Greetings.Length - 1]
                : this.lifecycleConfiguration.LastGreeting;
            var greetingIndex = Greetings.IndexOf(x => x == lastGreeting);
            if (greetingIndex == -1)
            {
                greetingIndex = Greetings.Length - 1;
            }

            greetingIndex++;
            this.greeting = Greetings[greetingIndex % Greetings.Length];
            this.lifecycleConfiguration.LastGreeting = this.greeting;
        }

        /// <inheritdoc />
        public IEnumerable<object> InputTargets => null;

        /// <inheritdoc />
        public void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Clear();
            renderContext.Home();
            renderContext.WriteLine($"{this.greeting} {this.lifecycleConfiguration.Name}");
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }
    }
}