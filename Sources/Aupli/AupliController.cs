// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AupliController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Threading.Tasks;
    using Aupli.Display;
    using Aupli.Volume;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// The main controller.
    /// </summary>
    public class AupliController
    {
        private readonly ControllerFactory controllerFactory;
        private readonly InputManager inputManager;
        private readonly IViewNavigator viewNavigator;
        private readonly ILogger logger;
        private IdleController idleController;

        /// <summary>
        /// Initializes a new instance of the <see cref="AupliController" /> class.
        /// </summary>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="viewNavigator">The textView navigator.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="log">The log.</param>
        public AupliController(InputManager inputManager, IViewNavigator viewNavigator, ControllerFactory controllerFactory, ILog log)
        {
            this.inputManager = inputManager;
            this.viewNavigator = viewNavigator;
            this.controllerFactory = controllerFactory;
            this.logger = log.GetCategorizedLogger(typeof(AupliController), true);
            this.controllerFactory.StartUpController.ViewRenderingInitialized += this.OnStartUpControllerViewRenderingInitialized;
            this.controllerFactory.MenuController.Exit += this.OnMenuControllerExit;
        }

        /// <summary>
        /// Starts the asynchronous.
        /// </summary>
        /// <returns>An sync task.</returns>
        public async Task StartAsync()
        {
            this.controllerFactory.StartUpController.Start();
            this.idleController = await this.controllerFactory.GetIdleControllerAsync();
            this.idleController.InputIdle += this.OnInputControllerInputIdle;
            this.idleController.Activated += this.OnInputControllerActive;
            this.idleController.Start();

            var shutdownController = await this.controllerFactory.GetShutdownControllerAsync();
            shutdownController.ShuttingDown += this.OnShutdownControllerShuttingDown;

            var volumeController = await this.controllerFactory.GetVolumeControllerAsync();
            volumeController.VolumeChanged += this.OnVolumeControllerVolumeChanged;
            await volumeController.StartAsync();

            var playerController = await this.controllerFactory.GetPlayerControllerAsync();
            await playerController.StartAsync();
            this.inputManager.StartFrame(playerController);
            this.inputManager.AddTarget(volumeController);
            await this.viewNavigator.NavigateToPlayerViewAsync();
            this.logger.LogDebug("Started");
        }

        private void OnStartUpControllerViewRenderingInitialized(object sender, EventArgs eventArgs)
        {
            this.logger.LogDebug("Navigate to startup");
            this.viewNavigator.NavigateToStartupView();
        }

        private async void OnInputControllerInputIdle(object sender, EventArgs eventArgs)
        {
            this.logger.LogDebug("Input idle");
            this.controllerFactory.DisplayController.SetBacklight(false);
            var playerController = await this.controllerFactory.GetPlayerControllerAsync();
            playerController.MenuRequested -= this.OnPlayerControllerMenuRequested;
        }

        private async void OnInputControllerActive(object sender, EventArgs eventArgs)
        {
            this.logger.LogDebug("Input active");
            this.controllerFactory.DisplayController.SetBacklight(true);
            var playerController = await this.controllerFactory.GetPlayerControllerAsync();
            playerController.MenuRequested += this.OnPlayerControllerMenuRequested;
        }

        private void OnVolumeControllerVolumeChanged(object sender, VolumeEventArgs e)
        {
            this.logger.LogDebug("Navigate to volume: ");
            this.viewNavigator.NavigateToVolumeViewAsync(TimeSpan.FromMilliseconds(1500));
        }

        private void OnShutdownControllerShuttingDown(object sender, EventArgs e)
        {
            this.logger.LogDebug("Navigate to shutdown");
            this.viewNavigator.NavigateToShutdownView();
        }

        private void OnPlayerControllerMenuRequested(object sender, EventArgs e)
        {
            this.logger.LogDebug("Navigate to menu");
            this.viewNavigator.NavigateToMenuView();
            this.inputManager.StartFrame(this.controllerFactory.MenuController);
        }

        private void OnMenuControllerExit(object sender, EventArgs e)
        {
            this.logger.LogDebug("Exit menu");
            this.inputManager.EndFrame();
            this.viewNavigator.GoBack();
        }
    }
}