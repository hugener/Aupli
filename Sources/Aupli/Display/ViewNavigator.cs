// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Display
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Serilog;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Navigator for the various views.
    /// </summary>
    /// <seealso cref="IViewNavigator" />
    public class ViewNavigator : IViewNavigator, IDisposable
    {
        private readonly Stack<ITextView> screenStack = new Stack<ITextView>();
        private readonly TextViewRenderer textViewRenderer;
        private readonly ViewFactory viewFactory;
        private readonly ILogger log;
        private readonly Pi.Timers.ITimer viewTimeoutTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNavigator" /> class.
        /// </summary>
        /// <param name="textViewRenderer">The textView renderer.</param>
        /// <param name="viewFactory">The textView factory.</param>
        /// <param name="logger">The log.</param>
        public ViewNavigator(TextViewRenderer textViewRenderer, ViewFactory viewFactory, ILogger logger)
        {
            this.textViewRenderer = textViewRenderer;
            this.viewFactory = viewFactory;
            this.log = logger.ForContext<ViewNavigator>();
            this.viewTimeoutTimer = Pi.Timers.Timer.Create();
            this.viewTimeoutTimer.Interval = Timeout.InfiniteTimeSpan;
            this.viewTimeoutTimer.Tick += (s, e) =>
            {
                this.viewTimeoutTimer.Stop();
                this.GoBack();
            };
        }

        /// <inheritdoc />
        public void NavigateToStartupView()
        {
            this.log.Debug("Navigate to StartUpTextView");
            this.textViewRenderer.TrySetView(this.viewFactory.StartUpTextView);
        }

        /// <inheritdoc />
        public async Task NavigateToPlayerViewAsync()
        {
            this.log.Debug("Navigate to PlayerTextView");
            var result = this.textViewRenderer.TrySetView(await this.viewFactory.GetPlayerTextViewAsync());
            if (result && result.Value != null)
            {
                this.screenStack.Push(result.Value);
            }
        }

        /// <inheritdoc />
        public async Task NavigateToVolumeViewAsync(TimeSpan activeTimeSpan)
        {
            this.viewTimeoutTimer.Stop();
            this.viewTimeoutTimer.Start(activeTimeSpan);
            this.log.Debug("Navigate to VolumeTextView");
            var result = this.textViewRenderer.TrySetView(await this.viewFactory.GetVolumeTextViewAsync());
            if (result && result.Value != null)
            {
                this.screenStack.Push(result.Value);
            }
        }

        /// <inheritdoc />
        public void NavigateToShutdownView()
        {
            this.log.Debug("Navigate to ShutdownTextView");
            this.textViewRenderer.TrySetView(this.viewFactory.ShutdownTextView);
        }

        /// <inheritdoc />
        public void NavigateToMenuView()
        {
            this.log.Debug("Navigate to MenuTextView");
            var result = this.textViewRenderer.TrySetView(this.viewFactory.MenuTextView);
            if (result && result.Value != null)
            {
                this.screenStack.Push(result.Value);
            }
        }

        /// <inheritdoc />
        public void GoBack()
        {
            if (this.screenStack.TryPop(out var screen))
            {
                this.log.Debug("Navigate back to: " + screen.GetType().Name);
                this.textViewRenderer.TrySetView(screen);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Pi.Timers.Timer.Dispose(this.viewTimeoutTimer);
        }
    }
}
