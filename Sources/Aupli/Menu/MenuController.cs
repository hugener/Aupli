// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Menu
{
    using System;
    using Aupli.Input;

    /// <summary>
    /// Controller for handling the menu.
    /// </summary>
    public class MenuController
    {
        private readonly InteractionController interactionController;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController"/> class.
        /// </summary>
        /// <param name="interactionController">The interaction controller.</param>
        public MenuController(InteractionController interactionController)
        {
            this.interactionController = interactionController;
            this.interactionController.KeyInputEvent.Register(this, this.OnInteractionControllerKeyInput);
            this.interactionController.TagInputEvent.Register(this, this.OnTagInput);
        }

        /// <summary>
        /// Occurs when menu should exit.
        /// </summary>
        public event EventHandler Exit;

        /// <summary>
        /// Occurs when a tag input occurs.
        /// </summary>
        public event EventHandler<TagInputArgs> TagInput;

        private void OnInteractionControllerKeyInput(object sender, KeyInputArgs keyInputArgs)
        {
            this.Exit?.Invoke(this, EventArgs.Empty);
        }

        private void OnTagInput(object sender, TagInputArgs e)
        {
            this.TagInput?.Invoke(this, e);
        }
    }
}