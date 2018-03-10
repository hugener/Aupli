// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Options.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.CommandLine
{
    using System;
    using Newtonsoft.Json;
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
        /// <param name="isLoggingToConsole">if set to <c>true</c> [is logging to console].</param>
        /// <param name="fileLogOptions">The file log options.</param>
        /// <param name="logLevel">The log level.</param>
        public Options(bool allowShutdown, bool isLoggingToConsole, LogLevel logLevel = LogLevel.Info, FileLogOptions fileLogOptions = null)
        {
            this.AllowShutdown = allowShutdown;
            this.IsLoggingToConsole = isLoggingToConsole;
            this.FileLogOptions = fileLogOptions;
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
        /// Gets a value indicating whether this instance is logging to console.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is logging to console; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoggingToConsole { get; private set; }

        /// <summary>
        /// Gets the file log options.
        /// </summary>
        /// <value>
        /// The file log options.
        /// </value>
        public FileLogOptions FileLogOptions { get; private set; }

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

            argumentsBuilder.AddSwitch(
                "cl",
                "console-log",
                this.IsLoggingToConsole,
                value => this.IsLoggingToConsole = value,
                "Specifies whether to use a Console logger");

            argumentsBuilder.AddOptional(
                "fl",
                "file-log",
                this.FileLogOptions,
                () => new FileLogOptions(),
                value => this.FileLogOptions = value,
                "Specifies whether to use a File logger and it's options");

            argumentsBuilder.AddOptional(
                "ll",
                "log-level",
                () => this.LogLevel.ToString(),
                value => this.LogLevel = value.ParseEnum<LogLevel>(),
                $"Specifies the log level: {string.Join(", ", Enum.GetNames(typeof(LogLevel)))}");
        }
    }
}