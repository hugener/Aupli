// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInteractionController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Interaction
{
    using Sundew.TextView.ApplicationFramework.Input;

    /// <summary>
    /// Interface for managing key and tag input events.
    /// </summary>
    public interface IInteractionController
    {
        /// <summary>
        /// Gets the key input event.
        /// </summary>
        /// <value>
        /// The key input event.
        /// </value>
        InputEvent<KeyInputArgs> KeyInputEvent { get; }

        /// <summary>
        /// Gets the tag input event.
        /// </summary>
        /// <value>
        /// The tag input event.
        /// </value>
        InputEvent<TagInputArgs> TagInputEvent { get; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();
    }
}