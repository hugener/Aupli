// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Volume
{
    using Aupli.ApplicationServices.Volume.Api;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Volume.Ari;

    /// <summary>
    /// Controls the volume of the MAX 9744 using the KY-040.
    /// </summary>
    public class VolumeController
    {
        private readonly IVolumeService volumeService;
        private readonly IInteractionController interactionController;
        private readonly IVolumeControllerReporter? volumeControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeController" /> class.
        /// </summary>
        /// <param name="volumeService">The volume service.</param>
        /// <param name="interactionController">The interaction controller.</param>
        /// <param name="volumeControllerReporter">The volume controller reporter.</param>
        public VolumeController(IVolumeService volumeService, IInteractionController interactionController, IVolumeControllerReporter? volumeControllerReporter)
        {
            this.volumeService = volumeService;
            this.interactionController = interactionController;
            this.volumeControllerReporter = volumeControllerReporter;
            this.volumeControllerReporter?.SetSource(this);
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
        }

        private void OnInteractionControllerKeyInput(object? sender, KeyInputArgs e)
        {
            this.volumeControllerReporter?.KeyInput(e.KeyInput);
            switch (e.KeyInput)
            {
                case KeyInput.Down:
                    this.volumeService.ChangeVolumeAsync(false).Wait();
                    break;
                case KeyInput.Up:
                    this.volumeService.ChangeVolumeAsync(true).Wait();
                    break;
                case KeyInput.Select:
                    this.volumeService.ToggleMuteAsync().Wait();
                    break;
            }
        }
    }
}
