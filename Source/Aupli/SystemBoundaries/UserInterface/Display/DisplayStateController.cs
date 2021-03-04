// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayStateController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Display
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Display.Ari;
    using Sundew.Base.Reporting;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.Navigation;

    /// <summary>
    /// Controls the display back light.
    /// </summary>
    public class DisplayStateController
    {
        private readonly ITextViewNavigator textViewNavigator;
        private readonly IDisplay display;
        private readonly IDisplayStateControllerReporter? displayStateControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayStateController" /> class.
        /// </summary>
        /// <param name="textViewNavigator">The text view navigator.</param>
        /// <param name="idleMonitor">The idle monitor.</param>
        /// <param name="display">The text display device.</param>
        /// <param name="displayStateControllerReporter">The display state controller reporter.</param>
        public DisplayStateController(ITextViewNavigator textViewNavigator, IIdleMonitor idleMonitor, IDisplay display, IDisplayStateControllerReporter? displayStateControllerReporter)
        {
            this.textViewNavigator = textViewNavigator;
            this.display = display;
            this.displayStateControllerReporter = displayStateControllerReporter;
            this.displayStateControllerReporter?.SetSource(this);
            idleMonitor.InputIdle += this.OnIdleControllerInputIdle;
            idleMonitor.Activated += this.OnIdleControllerActivated;
        }

        private async void OnIdleControllerInputIdle(object? sender, EventArgs eventArgs)
        {
            if (this.display.HasBacklight)
            {
                this.display.BacklightEnabled = false;
            }
            else
            {
                await this.textViewNavigator.ShowAsync(new DisabledDisplayTextView()).ConfigureAwait(false);
            }

            this.displayStateControllerReporter?.DisabledDisplay();
        }

        private async void OnIdleControllerActivated(object? sender, ActivatedEventArgs eventArgs)
        {
            if (this.display.HasBacklight)
            {
                if (!this.display.BacklightEnabled)
                {
                    this.display.BacklightEnabled = true;
                    this.displayStateControllerReporter?.EnabledDisplay();
                }
            }
            else if (!eventArgs.IsFirstActivation)
            {
                await this.textViewNavigator.NavigateBackAsync().ConfigureAwait(false);
                this.displayStateControllerReporter?.EnabledDisplay();
            }
        }
    }
}