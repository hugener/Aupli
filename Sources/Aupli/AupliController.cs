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
    using Serilog;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// The main controller.
    /// </summary>
    public class AupliController
    {
        private readonly ControllerFactory controllerFactory;
        private readonly InputManager inputManager;
        private readonly IViewNavigator viewNavigator;
        private readonly ILogger log;
        private IdleController idleController;

        /// <summary>
        /// Initializes a new instance of the <see cref="AupliController" /> class.
        /// </summary>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="viewNavigator">The textView navigator.</param>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="logger">The log.</param>
        public AupliController(InputManager inputManager, IViewNavigator viewNavigator, ControllerFactory controllerFactory, ILogger logger)
        {
            this.inputManager = inputManager;
            this.viewNavigator = viewNavigator;
            this.controllerFactory = controllerFactory;
            this.log = logger.ForContext<AupliController>();
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
            this.log.Debug("Started");
        }

        private void OnStartUpControllerViewRenderingInitialized(object sender, EventArgs eventArgs)
        {
            this.log.Debug("Navigate to startup");
            this.viewNavigator.NavigateToStartupView();
        }

        private async void OnInputControllerInputIdle(object sender, EventArgs eventArgs)
        {
            this.log.Debug("Input idle");
            this.controllerFactory.DisplayController.SetBacklight(false);
            var playerController = await this.controllerFactory.GetPlayerControllerAsync();
            playerController.MenuRequested -= this.OnPlayerControllerMenuRequested;
        }

        private async void OnInputControllerActive(object sender, EventArgs eventArgs)
        {
            this.log.Debug("Input active");
            this.controllerFactory.DisplayController.SetBacklight(true);
            var playerController = await this.controllerFactory.GetPlayerControllerAsync();
            playerController.MenuRequested += this.OnPlayerControllerMenuRequested;
        }

        private void OnVolumeControllerVolumeChanged(object sender, VolumeEventArgs e)
        {
            this.log.Debug("Navigate to volume: ");
            this.viewNavigator.NavigateToVolumeViewAsync(TimeSpan.FromMilliseconds(1500));
        }

        private void OnShutdownControllerShuttingDown(object sender, EventArgs e)
        {
            this.log.Debug("Navigate to shutdown");
            this.viewNavigator.NavigateToShutdownView();
        }

        private void OnPlayerControllerMenuRequested(object sender, EventArgs e)
        {
            this.log.Debug("Navigate to menu");
            this.viewNavigator.NavigateToMenuView();
            this.inputManager.StartFrame(this.controllerFactory.MenuController);
        }

        private void OnMenuControllerExit(object sender, EventArgs e)
        {
            this.log.Debug("Exit menu");
            this.inputManager.EndFrame();
            this.viewNavigator.GoBack();
        }
    }
}