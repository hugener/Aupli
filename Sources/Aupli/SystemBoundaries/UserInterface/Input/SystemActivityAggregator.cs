// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemActivityAggregator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Input
{
    using System;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.SystemBoundaries.UserInterface.Input.Ari;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Reports activity from the music player.
    /// </summary>
    /// <seealso cref="IActivityAggregator" />
    public class SystemActivityAggregator : IActivityAggregator
    {
        private readonly IPlayerStatusUpdater playerStatusUpdater;
        private readonly ISystemActivityAggregatorReporter systemActivityAggregatorReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemActivityAggregator" /> class.
        /// </summary>
        /// <param name="playerStatusUpdater">The player status.</param>
        /// <param name="systemActivityAggregatorReporter">The system activity aggregator reporter.</param>
        public SystemActivityAggregator(IPlayerStatusUpdater playerStatusUpdater, ISystemActivityAggregatorReporter systemActivityAggregatorReporter)
        {
            this.playerStatusUpdater = playerStatusUpdater;
            this.systemActivityAggregatorReporter = systemActivityAggregatorReporter;
            this.systemActivityAggregatorReporter?.SetSource(this);
            this.playerStatusUpdater.StatusChanged += this.OnPlayerStatusUpdaterStatusChanged;
        }

        /// <summary>
        /// Occurs when an activity happens.
        /// </summary>
        public event EventHandler<EventArgs> ActivityOccured;

        private void OnPlayerStatusUpdaterStatusChanged(object sender, StatusEventArgs e)
        {
            if (e.State == PlayerState.Playing)
            {
                this.systemActivityAggregatorReporter?.Playing(e.State, e.Artist, e.Title, e.Elapsed);
                this.ActivityOccured?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}