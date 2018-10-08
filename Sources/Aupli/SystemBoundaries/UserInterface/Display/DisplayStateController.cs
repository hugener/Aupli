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
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Controls the display back light.
    /// </summary>
    public class DisplayStateController
    {
        private readonly IDisplay display;
        private readonly IDisplayStateControllerReporter displayStateControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayStateController" /> class.
        /// </summary>
        /// <param name="idleController">The idle controller.</param>
        /// <param name="display">The text display device.</param>
        /// <param name="displayStateControllerReporter">The display state controller reporter.</param>
        public DisplayStateController(IdleController idleController, IDisplay display, IDisplayStateControllerReporter displayStateControllerReporter)
        {
            this.display = display;
            this.displayStateControllerReporter = displayStateControllerReporter;
            this.displayStateControllerReporter?.SetSource(this);
            idleController.InputIdle += this.OnIdleControllerInputIdle;
            idleController.Activated += this.OnIdleControllerActive;
        }

        private void OnIdleControllerInputIdle(object sender, EventArgs eventArgs)
        {
            this.display.IsEnabled = false;
            this.displayStateControllerReporter?.DisabledDisplay();
        }

        private void OnIdleControllerActive(object sender, EventArgs eventArgs)
        {
            this.display.IsEnabled = true;
            this.displayStateControllerReporter?.EnabledDisplay();
        }
    }
}