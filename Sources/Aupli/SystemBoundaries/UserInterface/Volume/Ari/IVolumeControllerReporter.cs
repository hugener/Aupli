﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Volume.Ari
{
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter interface for <see cref="VolumeController"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IVolumeControllerReporter : IReporter
    {
        /// <summary>
        /// Keys the input.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        void KeyInput(KeyInput keyInput);
    }
}