// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicPlayerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Mpc
{
    using System;
    using System.Threading;
    using Aupli.Mpc;
    using MpcNET;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// Logger for the music player.
    /// </summary>
    /// <seealso cref="IMusicPlayerReporter" />
    /// <seealso cref="IMpcConnectionReporter" />
    public class MusicPlayerLogger : IMusicPlayerReporter, IMpcConnectionReporter
    {
        private readonly ILogger log;
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
            this.log.Write(this.currentLogLevel, "Starting playlist: {Playlist} | {ThreadId}", playlistName, GetThreadId());
        }

        /// <summary>
        /// Ignoreds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void IgnoredPlaylist(string playlistName)
        {
            this.log.Write(this.currentLogLevel, "Ignored playlist: {Playlist} | {ThreadId}", string.IsNullOrEmpty(playlistName) ? "<None>" : playlistName, GetThreadId());
        }

        /// <summary>
        /// Logs connecting.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void Connecting(bool isReconnect, int connectAttempt)
        {
            this.log.Write(this.currentLogLevel, "{ConnectionMethod}, {Attempt} | {ThreadId}", isReconnect ? "Reconnecting" : "Connecting", connectAttempt, GetThreadId());
        }

        /// <summary>
        /// Logs connection accepted.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void ConnectionAccepted(bool isReconnect, int connectAttempt)
        {
            this.log.Write(this.currentLogLevel, "Connection Accepted {ConnectionMethod}, {Attempt} | {ThreadId}", isReconnect ? "Reconnecting" : "Connecting", connectAttempt, GetThreadId());
        }

        /// <summary>
        /// Logs connected.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        /// <param name="connectionInfo">The connection information.</param>
        public void Connected(bool isReconnect, int connectAttempt, string connectionInfo)
        {
            this.log.Write(this.currentLogLevel, "Connected: {ConnectAttempt} - {ConnectionInfo} | {ThreadId}", connectAttempt, connectionInfo, GetThreadId());
        }

        /// <summary>
        /// Logs the send command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Sending(string command)
        {
            this.log.Write(this.currentLogLevel, "Sending: {Command} | {ThreadId}", command, GetThreadId());
        }

        /// <summary>
        /// Logs the send exception.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        /// <param name="exception">The exception.</param>
        public void SendException(string command, int sendAttempt, Exception exception)
        {
            this.log.Write(this.currentLogLevel, "Sending: {Command} - attempt: {Attempt} | {ThreadId} | {Exception}", command, sendAttempt, GetThreadId(), exception);
        }

        /// <summary>
        /// Write send retry.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        public void RetrySend(string command, int sendAttempt)
        {
            this.log.Write(this.currentLogLevel, "Sending: {Command} - attempt: {Attempt} | {ThreadId}", command, sendAttempt, GetThreadId());
        }

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <param name="responseLine">The response line.</param>
        public void ReadResponse(string responseLine)
        {
            this.log.Write(this.currentLogLevel, "ReadResponse: {Content} | {ThreadId}", responseLine, GetThreadId());
        }

        /// <summary>
        /// Logs disconnecting.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnecting(bool isExplicitDisconnect)
        {
            this.log.Write(this.currentLogLevel, "Disconnecting: {Reason} | {ThreadId}", isExplicitDisconnect ? "explicit" : "implicit", GetThreadId());
        }

        /// <summary>
        /// Logs disconnected.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnected(bool isExplicitDisconnect)
        {
            this.log.Write(this.currentLogLevel, "Disconnected: {Reason} | {ThreadId}", isExplicitDisconnect ? "explicit" : "implicit", GetThreadId());
        }

        private static int GetThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
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