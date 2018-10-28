// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Menu
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Sundew.Pi.ApplicationFramework.Navigation;

    /// <summary>
    /// Controller for handling the menu.
    /// </summary>
    public class MenuController
    {
        private readonly IInteractionController interactionController;
        private readonly ITextViewNavigator textViewNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuController" /> class.
        /// </summary>
        /// <param name="interactionController">The interaction controller.</param>
        /// <param name="textViewNavigator">The text view navigator.</param>
        public MenuController(IInteractionController interactionController, ITextViewNavigator textViewNavigator)
        {
            this.interactionController = interactionController;
            this.textViewNavigator = textViewNavigator;
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
            this.textViewNavigator.NavigateBackAsync();
            this.Exit?.Invoke(this, EventArgs.Empty);
        }

        private void OnTagInput(object sender, TagInputArgs e)
        {
            this.TagInput?.Invoke(this, e);
        }
    }
}