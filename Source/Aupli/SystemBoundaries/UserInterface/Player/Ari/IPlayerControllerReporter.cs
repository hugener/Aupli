// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayerControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Player.Ari
{
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter for <see cref="PlayerController"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IPlayerControllerReporter : IReporter
    {
        /// <summary>
        /// Keys the input.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        void KeyInput(KeyInput keyInput);

        /// <summary>
        /// Tags the input.
        /// </summary>
        /// <param name="tagInputArgs">The tag input arguments.</param>
        void TagInput(TagInputArgs tagInputArgs);
    }
}