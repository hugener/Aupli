// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartUpTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Lifecycle
{
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
        private readonly ILifecycleConfiguration startupConfiguration;
        private readonly string greeting;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpTextView" /> class.
        /// </summary>
        /// <param name="startupConfiguration">The startup configuration.</param>
        public StartUpTextView(ILifecycleConfiguration startupConfiguration)
        {
            this.startupConfiguration = startupConfiguration;
            var lastGreeting = string.IsNullOrEmpty(this.startupConfiguration.LastGreeting)
                ? Greetings[Greetings.Length - 1]
                : this.startupConfiguration.LastGreeting;
            var greetingIndex = Greetings.IndexOf(x => x == lastGreeting);
            if (greetingIndex == -1)
            {
                greetingIndex = Greetings.Length - 1;
            }

            greetingIndex++;
            this.greeting = Greetings[greetingIndex % Greetings.Length];
            this.startupConfiguration.LastGreeting = this.greeting;
        }

        /// <inheritdoc />
        public void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Clear();
            renderContext.Home();
            renderContext.WriteLine($"{this.greeting} {this.startupConfiguration.Name}");
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }
    }
}