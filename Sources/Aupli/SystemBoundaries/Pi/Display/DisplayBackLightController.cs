// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayBackLightController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Display
{
    using System;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Controls the display back light.
    /// </summary>
    public class DisplayBackLightController
    {
        private readonly IDisplay display;
        private readonly IDisplayBacklightControllerReporter displayBacklightControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayBackLightController" /> class.
        /// </summary>
        /// <param name="idleController">The idle controller.</param>
        /// <param name="display">The text display device.</param>
        /// <param name="displayBacklightControllerReporter">The display backlight controller reporter.</param>
        public DisplayBackLightController(IdleController idleController, IDisplay display, IDisplayBacklightControllerReporter displayBacklightControllerReporter)
        {
            this.display = display;
            this.displayBacklightControllerReporter = displayBacklightControllerReporter;
            this.displayBacklightControllerReporter?.SetSource(this);
            idleController.InputIdle += this.OnIdleControllerInputIdle;
            idleController.Activated += this.OnIdleControllerActive;
        }

        private void OnIdleControllerInputIdle(object sender, EventArgs eventArgs)
        {
            this.display.BacklightEnabled = false;
            this.displayBacklightControllerReporter?.DisabledBacklight();
        }

        private void OnIdleControllerActive(object sender, EventArgs eventArgs)
        {
            this.display.BacklightEnabled = true;
            this.displayBacklightControllerReporter?.EnabledBacklight();
        }
    }
}