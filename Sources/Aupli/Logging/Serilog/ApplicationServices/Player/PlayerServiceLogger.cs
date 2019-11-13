// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerServiceLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.ApplicationServices.Player
{
    using System.Reflection;
    using Aupli.ApplicationServices.Player.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IPlayerServiceReporter"/>.
    /// </summary>
    /// <seealso cref="IPlayerServiceReporter" />
    public class PlayerServiceLogger : IPlayerServiceReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerServiceLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PlayerServiceLogger(ILogger logger)
        {
            this.log = logger.ForContext<PlayerServiceLogger>();
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
        /// Founds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void FoundPlaylist(string playlistName)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{{nameof(playlistName)}}}", playlistName);
        }

        /// <summary>
        /// Dids the not find playlist for identifier.
        /// </summary>
        /// <param name="playlistId">The playlist identifier.</param>
        public void DidNotFindPlaylistForId(string playlistId)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{{nameof(playlistId)}}}", playlistId);
        }
    }
}