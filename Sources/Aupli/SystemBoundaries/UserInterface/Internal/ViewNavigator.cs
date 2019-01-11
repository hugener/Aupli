// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume.Api;
    using Aupli.SystemBoundaries.UserInterface.Api;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using Aupli.SystemBoundaries.UserInterface.Shutdown.Api;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using Sundew.Base.Threading;
    using Sundew.TextView.ApplicationFramework.Navigation;

    /// <summary>
    /// Navigator for the various views.
    /// </summary>
    internal class ViewNavigator : IViewNavigator
    {
        private readonly ITextViewNavigator textViewNavigator;
        private readonly ViewFactory viewFactory;
        private readonly ITimerFactory timerFactory;
        private readonly VolumeController volumeController;
        private readonly IViewNavigatorReporter viewNavigatorReporter;
        private readonly ITimer viewTimeoutTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNavigator" /> class.
        /// </summary>
        /// <param name="volumeChangeNotifier">The volume change notifier.</param>
        /// <param name="menuRequester">The menu requester.</param>
        /// <param name="shutdownNotifier">The shutdown notifier.</param>
        /// <param name="volumeController">The volume controller.</param>
        /// <param name="textViewNavigator">The text view navigator.</param>
        /// <param name="viewFactory">The textView factory.</param>
        /// <param name="timerFactory">The timer factory.</param>
        /// <param name="viewNavigatorReporter">The view navigator reporter.</param>
        internal ViewNavigator(
            IVolumeChangeNotifier volumeChangeNotifier,
            MenuRequester menuRequester,
            IShutdownNotifier shutdownNotifier,
            VolumeController volumeController,
            ITextViewNavigator textViewNavigator,
            ViewFactory viewFactory,
            ITimerFactory timerFactory,
            IViewNavigatorReporter viewNavigatorReporter = null)
        {
            this.textViewNavigator = textViewNavigator;
            this.volumeController = volumeController;
            this.viewFactory = viewFactory;
            this.timerFactory = timerFactory;
            this.viewNavigatorReporter = viewNavigatorReporter;
            this.viewNavigatorReporter?.SetSource(this);
            this.viewTimeoutTimer = timerFactory.Create();
            this.viewTimeoutTimer.Tick += async timer =>
            {
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
            await this.textViewNavigator.NavigateToModalAsync(this.viewFactory.PlayerTextView, oldTextView => this.viewNavigatorReporter?.NavigateToPlayerTextView(), this.volumeController);
        }

        /// <summary>
        /// Navigates the back asynchronous.
        /// </summary>
        /// <returns>A async task.</returns>
        public async Task NavigateBackAsync()
        {
            await this.textViewNavigator.NavigateBackAsync(oldViewView => this.viewNavigatorReporter?.NavigateBack());
        }

        private async void OnVolumeControllerVolumeChanged(object sender, EventArgs e)
        {
            this.viewTimeoutTimer.StartOnce(TimeSpan.FromMilliseconds(1500));
            await this.textViewNavigator.ShowAsync(this.viewFactory.VolumeTextView, oldTextView => this.viewNavigatorReporter?.NavigateToVolumeTextView());
        }

        private async void OnShutdownNotifierShuttingDown(object sender, EventArgs e)
        {
            await this.textViewNavigator.NavigateToAsync(this.viewFactory.ShutdownTextView, oldTextView => this.viewNavigatorReporter?.NavigateToShutdownTextView());
        }

        private async void OnMenuRequesterMenuRequested(object sender, EventArgs e)
        {
            await this.textViewNavigator.NavigateToModalAsync(this.viewFactory.MenuTextView, oldTextView => this.viewNavigatorReporter?.NavigateToMenuTextView());
        }
    }
}
