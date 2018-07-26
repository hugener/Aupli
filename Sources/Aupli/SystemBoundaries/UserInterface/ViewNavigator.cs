// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Interface;
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using global::Pi.Timers;
    using Sundew.Pi.ApplicationFramework.Navigation;

    /// <summary>
    /// Navigator for the various views.
    /// </summary>
    public class ViewNavigator : IDisposable
    {
        private readonly TextViewNavigator textViewNavigator;
        private readonly IViewFactory viewFactory;
        private readonly IViewNavigatorReporter viewNavigatorReporter;
        private readonly ITimer viewTimeoutTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNavigator" /> class.
        /// </summary>
        /// <param name="volumeChangeNotifier">The volume change notifier.</param>
        /// <param name="menuRequester">The menu requester.</param>
        /// <param name="shutdownNotifier">The shutdown notifier.</param>
        /// <param name="textViewNavigator">The text view navigator.</param>
        /// <param name="viewFactory">The textView factory.</param>
        /// <param name="timerFactory">The timer factory.</param>
        /// <param name="viewNavigatorReporter">The view navigator reporter.</param>
        public ViewNavigator(
            IVolumeChangeNotifier volumeChangeNotifier,
            IMenuRequester menuRequester,
            IShutdownNotifier shutdownNotifier,
            TextViewNavigator textViewNavigator,
            IViewFactory viewFactory,
            ITimerFactory timerFactory,
            IViewNavigatorReporter viewNavigatorReporter = null)
        {
            this.textViewNavigator = textViewNavigator;
            this.viewFactory = viewFactory;
            this.viewNavigatorReporter = viewNavigatorReporter;
            this.viewNavigatorReporter?.SetSource(this);
            this.viewTimeoutTimer = timerFactory.Create();
            this.viewTimeoutTimer.Interval = Timeout.InfiniteTimeSpan;
            this.viewTimeoutTimer.Tick += async (s, e) =>
            {
                this.viewTimeoutTimer.Stop();
                await this.NavigateBackAsync();
            };
            volumeChangeNotifier.VolumeChanged += this.OnVolumeControllerVolumeChanged;
            menuRequester.MenuRequested += this.OnMenuRequesterMenuRequested;
            shutdownNotifier.ShuttingDown += this.OnShutdownNotifierShuttingDown;
        }

        /// <summary>
        /// Navigates to player view asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        public async Task NavigateToPlayerViewAsync()
        {
            this.viewNavigatorReporter?.NavigateToPlayerTextView();
            await this.textViewNavigator.NavigateToModalAsync(this.viewFactory.PlayerTextView, this.viewFactory.VolumeTextView.InputTargets);
        }

        /// <summary>
        /// Navigates the back asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        public async Task NavigateBackAsync()
        {
            this.viewNavigatorReporter?.NavigateBack();
            await this.textViewNavigator.NavigateBackAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            global::Pi.Timers.Timer.Dispose(this.viewTimeoutTimer);
        }

        private async void OnVolumeControllerVolumeChanged(object sender, EventArgs e)
        {
            this.viewTimeoutTimer.Stop();
            this.viewTimeoutTimer.Start(TimeSpan.FromMilliseconds(1500));
            this.viewNavigatorReporter?.NavigateToVolumeTextView();
            await this.textViewNavigator.NavigateToAsync(this.viewFactory.VolumeTextView);
        }

        private async void OnShutdownNotifierShuttingDown(object sender, EventArgs e)
        {
            this.viewNavigatorReporter?.NavigateToShutdownTextView();
            await this.textViewNavigator.NavigateToAsync(this.viewFactory.ShutdownTextView);
        }

        private async void OnMenuRequesterMenuRequested(object sender, EventArgs e)
        {
            this.viewNavigatorReporter?.NavigateToMenuTextView();
            await this.textViewNavigator.NavigateToModalAsync(this.viewFactory.MenuTextView);
        }
    }
}
