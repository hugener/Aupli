// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlsModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Controls
{
    using Aupli.ApplicationServices.Volume.Ari;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.Bridges.Shutdown;

    /// <summary>
    /// Required interface with various controls for user interface module.
    /// </summary>
    public interface IControlsModule
    {
        /// <summary>
        /// Gets the system control.
        /// </summary>
        /// <value>
        /// The system control.
        /// </value>
        ISystemControl SystemControl { get; }

        /// <summary>
        /// Gets the input controls.
        /// </summary>
        /// <value>
        /// The input controls.
        /// </value>
        InputControls InputControls { get; }

        /// <summary>
        /// Gets the amplifier.
        /// </summary>
        /// <value>
        /// The amplifier.
        /// </value>
        IAmplifier Amplifier { get; }
    }
}