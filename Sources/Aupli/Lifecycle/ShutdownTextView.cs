// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Lifecycle
{
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <inheritdoc />
    /// <summary>
    /// View for rendering the shutdown messages.
    /// </summary>
    /// <seealso cref="T:Aupli.ViewRendering.ITextView" />
    public class ShutdownTextView : ITextView
    {
        private readonly ILifecycleConfiguration startupConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownTextView"/> class.
        /// </summary>
        /// <param name="startupConfiguration">The startup configuration.</param>
        public ShutdownTextView(ILifecycleConfiguration startupConfiguration)
        {
            this.startupConfiguration = startupConfiguration;
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
            renderContext.WriteLine("Bye bye " + this.startupConfiguration.Name);
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }
    }
}