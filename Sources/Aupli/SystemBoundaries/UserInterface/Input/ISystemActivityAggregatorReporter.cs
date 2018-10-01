// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemActivityAggregatorReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Input
{
    using System;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for receiving reports from <see cref="SystemActivityAggregator"/>.
    /// </summary>
    public interface ISystemActivityAggregatorReporter : IReporter
    {
        /// <summary>
        /// Playings the specified player state.
        /// </summary>
        /// <param name="playerState">State of the player.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="elapsed">The elapsed.</param>
        void Playing(PlayerState playerState, string artist, string title, TimeSpan elapsed);
    }
}