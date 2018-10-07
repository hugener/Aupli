﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Shutdown
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <inheritdoc />
    /// <summary>
    /// View for rendering the shutdown messages.
    /// </summary>
    /// <seealso cref="T:Aupli.ViewRendering.ITextView" />
    public class ShutdownTextView : ITextView
    {
        private readonly ILifecycleConfiguration lifecycleConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownTextView"/> class.
        /// </summary>
        /// <param name="lifecycleConfiguration">The startup configuration.</param>
        public ShutdownTextView(ILifecycleConfiguration lifecycleConfiguration)
        {
            this.lifecycleConfiguration = lifecycleConfiguration;
        }

        /// <inheritdoc />
        public IEnumerable<object> InputTargets => null;

        /// <inheritdoc />
        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Clear();
            renderContext.Home();
            renderContext.WriteLine("Bye bye " + this.lifecycleConfiguration.Name);
        }

        /// <inheritdoc />
        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }
    }
}