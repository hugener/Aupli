// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemActivityAggregator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Input
{
    using System;
    using Aupli.Mpc;
    using Serilog;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Reports activity from the music player.
    /// </summary>
    /// <seealso cref="IActivityAggregator" />
    public class SystemActivityAggregator : IActivityAggregator
    {
        private readonly IPlayerInfo playerInfo;
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemActivityAggregator" /> class.
        /// </summary>
        /// <param name="playerInfo">The player status.</param>
        /// <param name="logger">The log.</param>
        public SystemActivityAggregator(IPlayerInfo playerInfo, ILogger logger)
        {
            this.playerInfo = playerInfo;
            this.log = logger.ForContext<SystemActivityAggregator>();
            this.playerInfo.StatusChanged += this.OnPlayerInfoStatusChanged;
        }

        /// <summary>
        /// Occurs when an activity happens.
        /// </summary>
        public event EventHandler<EventArgs> ActivityOccured;

        private void OnPlayerInfoStatusChanged(object sender, StatusEventArgs e)
        {
            if (e.State == PlayerState.Playing)
            {
                this.log.Verbose("Player status: {State}: {Artist} - {Title} | {Elapsed}", e.State, e.Artist, e.Title, e.Elapsed);
                this.ActivityOccured?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}