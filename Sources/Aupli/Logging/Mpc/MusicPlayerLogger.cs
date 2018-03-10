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
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Logger for the music player.
    /// </summary>
    /// <seealso cref="IMusicPlayerObserver" />
    /// <seealso cref="IMpcConnectionObserver" />
    public class MusicPlayerLogger : IMusicPlayerObserver, IMpcConnectionObserver
    {
        private readonly ILogger logger;
        private LogLevel currentLogLevel = LogLevel.Debug;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayerLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public MusicPlayerLogger(ILog log)
        {
            this.logger = log.GetCategorizedLogger(this.GetType().Name);
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
            this.currentLogLevel = LogLevel.Trace;
            return new LogLevelResetter(this, previousLogLevel);
        }

        /// <summary>
        /// Startings the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void StartingPlaylist(string playlistName)
        {
            this.logger.Log(this.currentLogLevel, "Starting playlist: " + playlistName);
        }

        /// <summary>
        /// Ignoreds the playlist.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        public void IgnoredPlaylist(string playlistName)
        {
            this.logger.Log(this.currentLogLevel, "Ignored playlist: " + (string.IsNullOrEmpty(playlistName) ? "<None>" : playlistName));
        }

        /// <summary>
        /// Logs connecting.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void Connecting(bool isReconnect, int connectAttempt)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | {(isReconnect ? "Reconnecting" : "Connecting")}: #{connectAttempt}");
        }

        /// <summary>
        /// Logs connection accepted.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        public void ConnectionAccepted(bool isReconnect, int connectAttempt)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId}  | Connection: #{connectAttempt}");
        }

        /// <summary>
        /// Logs connected.
        /// </summary>
        /// <param name="isReconnect">if set to <c>true</c> [is reconnect].</param>
        /// <param name="connectAttempt">The connect attempt.</param>
        /// <param name="connectionInfo">The connection information.</param>
        public void Connected(bool isReconnect, int connectAttempt, string connectionInfo)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Connected: {connectAttempt} - {connectionInfo}");
        }

        /// <summary>
        /// Logs the send command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Sending(string command)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Sending: {command}");
        }

        /// <summary>
        /// Logs the send exception.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        /// <param name="exception">The exception.</param>
        public void SendException(string command, int sendAttempt, Exception exception)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Sending: {command}: attempt: {sendAttempt} - {exception}");
        }

        /// <summary>
        /// Write send retry.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sendAttempt">The send attempt.</param>
        public void RetrySend(string command, int sendAttempt)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Sending: {command}: attempt: {sendAttempt}");
        }

        /// <summary>
        /// Reads the response.
        /// </summary>
        /// <param name="responseLine">The response line.</param>
        public void ReadResponse(string responseLine)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | ReadResponse: {responseLine}");
        }

        /// <summary>
        /// Logs disconnecting.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnecting(bool isExplicitDisconnect)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Disconnecting: {(isExplicitDisconnect ? "explicit" : "implicit")}");
        }

        /// <summary>
        /// Logs disconnected.
        /// </summary>
        /// <param name="isExplicitDisconnect">if set to <c>true</c> [is explicit].</param>
        public void Disconnected(bool isExplicitDisconnect)
        {
            this.logger.Log(this.currentLogLevel, $"Tid:{Thread.CurrentThread.ManagedThreadId} | Disconnected: {(isExplicitDisconnect ? "explicit" : "implicit")}");
        }

        private class LogLevelResetter : IDisposable
        {
            private readonly MusicPlayerLogger musicPlayerLogger;
            private readonly LogLevel previousLogLevel;

            public LogLevelResetter(MusicPlayerLogger musicPlayerLogger, LogLevel previousLogLevel)
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