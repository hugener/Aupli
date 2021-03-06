﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicPlayerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.MusicControl
{
    using System;
    using Aupli.SystemBoundaries.MusicControl.Ari;
    using global::MpcNET;
    using global::Serilog;
    using global::Serilog.Events;
    using Sundew.Base;

    /// <summary>
    /// Logger for the music player.
    /// </summary>
    /// <seealso cref="IMusicPlayerReporter" />
    /// <seealso cref="IMpcConnectionReporter" />
    public class MusicPlayerLogger : IMusicPlayerReporter
    {
        private ILogger log;
        private LogEventLevel currentLogLevel = LogEventLevel.Debug;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayerLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MusicPlayerLogger(ILogger logger)
        {
            this.log = logger.ForContext(this.GetType());
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Changes the log level to trace.
        /// </summary>
        /// <returns>
        /// A disposable to reset the logging.
        /// </returns>
        public IDisposable EnterStatusRefresh()
        {
            var previousLogLevel = this.currentLogLevel;
            this.currentLogLevel = LogEventLevel.Verbose;
            return new LogLevelResetter(this, previousLogLevel);
        }

        /// <summary>
        /// Startings the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void StartingPlaylist(string playlistName)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Starting playlist: {Playlist}", playlistName));
        }

        /// <summary>
        /// Called when [status exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void OnStatusException(Exception exception)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Status Exception: {Exception}", exception));
        }

        /// <summary>
        /// Ignoreds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void IgnoredPlaylist(string playlistName)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Ignored playlist: {Playlist}", string.IsNullOrEmpty(playlistName) ? "<None>" : playlistName));
        }

        /// <summary>
        /// Logs connecting.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void Connecting(bool isReconnect, int connectAttempt)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "{ConnectionMethod}, {Attempt}", isReconnect ? "Reconnecting" : "Connecting", connectAttempt));
        }

        /// <summary>
        /// Logs connection accepted.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void ConnectionAccepted(bool isReconnect, int connectAttempt)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Connection Accepted {ConnectionMethod}, {Attempt}", isReconnect ? "Reconnecting" : "Connecting", connectAttempt));
        }

        /// <summary>
        /// Logs connected.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        /// <param name="connectionInfo">The connection information.</param>
        public void Connected(bool isReconnect, int connectAttempt, string connectionInfo)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Connected: {ConnectAttempt} - {ConnectionInfo}", connectAttempt, connectionInfo));
        }

        /// <summary>
        /// Logs the send command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Sending(string command)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Sending: {Command}", command));
        }

        /// <summary>
        /// Logs the send exception.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        /// <param name="exception">The exception.</param>
        public void SendException(string command, int sendAttempt, Exception exception)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Send exception: {Command} - attempt: {Attempt} | {Exception}", command, sendAttempt, exception));
        }

        /// <summary>
        /// Write send retry.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        public void RetrySend(string command, int sendAttempt)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Retry send: {Command} - attempt: {Attempt}", command, sendAttempt));
        }

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <param name="responseLine">The response line.</param>
        /// <param name="command">The command.</param>
        public void ReadResponse(string responseLine, string command)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "ReadResponse: {Content} for {Command}", responseLine, command));
        }

        /// <summary>
        /// Logs disconnecting.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnecting(bool isExplicitDisconnect)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Disconnecting: {Reason}", isExplicitDisconnect ? "explicit" : "implicit"));
        }

        /// <summary>
        /// Logs disconnected.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnected(bool isExplicitDisconnect)
        {
            this.Log(() => this.log.Write(this.currentLogLevel, "Disconnected: {Reason}", isExplicitDisconnect ? "explicit" : "implicit"));
        }

        private void Log(Action logAction)
        {
            if (this.currentLogLevel != LogEventLevel.Verbose)
            {
                logAction();
            }
        }

        private class LogLevelResetter : IDisposable
        {
            private readonly MusicPlayerLogger musicPlayerLogger;
            private readonly LogEventLevel previousLogLevel;

            public LogLevelResetter(MusicPlayerLogger musicPlayerLogger, LogEventLevel previousLogLevel)
            {
                this.musicPlayerLogger = musicPlayerLogger;
                this.previousLogLevel = previousLogLevel;
            }

            public void Dispose()
            {
                this.musicPlayerLogger.currentLogLevel = this.previousLogLevel;
            }
        }
    }
}