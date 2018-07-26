// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemActivityAggregatorLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Input
{
    using System;
    using Aupli.ApplicationServices.RequiredInterface.Player;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="ISystemActivityAggregatorReporter"/>.
    /// </summary>
    /// <seealso cref="ISystemActivityAggregatorReporter" />
    public class SystemActivityAggregatorLogger : ISystemActivityAggregatorReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemActivityAggregatorLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SystemActivityAggregatorLogger(ILogger logger)
        {
            this.log = logger.ForContext<SystemActivityAggregatorLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Playings the specified player state.
        /// </summary>
        /// <param name="playerState">State of the player.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="elapsed">The elapsed.</param>
        public void Playing(PlayerState playerState, string artist, string title, TimeSpan elapsed)
        {
            this.log.Verbose("Player status: {State}: {Artist} - {Title} | {Elapsed}", playerState, artist, title, elapsed);
        }
    }
}