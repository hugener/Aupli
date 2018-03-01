// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILog.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    /// <summary>
    /// Interface for implementing a log.
    /// </summary>
    public interface ILog : IDisposable
    {
        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the categorized logger.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>The categorized logger.</returns>
        ILogger GetCategorizedLogger(string category);

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="category">The category.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="message">The message.</param>
        void LogMessage(LogLevel logLevel, string category, DateTime dateTime, string message);
    }
}