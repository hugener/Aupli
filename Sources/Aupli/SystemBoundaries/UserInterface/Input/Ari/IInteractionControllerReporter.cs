// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInteractionControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Input.Ari
{
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter for <see cref="InteractionController"/>.
    /// </summary>
    public interface IInteractionControllerReporter : IReporter
    {
        /// <summary>
        /// Starteds this instance.
        /// </summary>
        void Started();

        /// <summary>
        /// Tags the input event.
        /// </summary>
        /// <param name="uid">The uid.</param>
        void TagInputEvent(string uid);

        /// <summary>
        /// Keys the input event.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        void KeyInputEvent(KeyInput keyInput);
    }
}