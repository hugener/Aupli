// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine
{
    using System;
    using Sundew.Base.Enumerations;
    using Sundew.CommandLine;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Defines the commandLine options and parsing.
    /// </summary>
    /// <seealso cref="Sundew.CommandLine.IArguments" />
    public class Options : IArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Options" /> class.
        /// </summary>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="logPath">The log path.</param>
        /// <param name="logLevel">The log level.</param>
        public Options(bool allowShutdown, LogType logType, string logPath, LogLevel logLevel)
        {
            this.AllowShutdown = allowShutdown;
            this.LogType = logType;
            this.LogPath = logPath;
            this.LogLevel = logLevel;
        }

        /// <summary>
        /// Gets a value indicating whether [allow shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow shutdown]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowShutdown { get; private set; }

        /// <summary>
        /// Gets the type of the log.
        /// </summary>
        /// <value>
        /// The type of the log.
        /// </value>
        public LogType LogType { get; private set; }

        /// <summary>
        /// Gets the log path.
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        public string LogPath { get; private set; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public LogLevel LogLevel { get; private set; }

        /// <summary>
        /// Configures the specified arguments builder.
        /// </summary>
        /// <param name="argumentsBuilder">The arguments builder.</param>
        public void Configure(IArgumentsBuilder argumentsBuilder)
        {
            argumentsBuilder.AddSwitch(
                "s",
                "shutdown",
                this.AllowShutdown,
                b => this.AllowShutdown = b,
                "Allows Aupli to shutdown the device when closing.");

            argumentsBuilder.AddOptional(
                "l",
                "log",
                () => this.LogType.ToString(),
                value => this.LogType = value.ParseFlagsEnum<LogType>(),
                "Specifies whether to use a File- or Console logger");

            argumentsBuilder.AddOptional(
                "lp",
                "log-path",
                () => this.LogPath,
                value => this.LogPath = value,
                "Specifies the log path, in case of the File logger");

            argumentsBuilder.AddOptional(
                "ll",
                "log-level",
                () => this.LogLevel.ToString(),
                value => this.LogLevel = value.ParseEnum<LogLevel>(),
                $"Specifies the log level: {string.Join(", ", Enum.GetNames(typeof(LogLevel)))}");
        }
    }
}