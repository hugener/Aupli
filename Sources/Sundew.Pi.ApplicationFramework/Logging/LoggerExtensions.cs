// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    /// <summary>
    /// Extends <see cref="ILogger"/> with easy to use methods.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        public static void Log(this ILogger logger, LogLevel logLevel, string message)
        {
            logger.Log(logLevel, DateTime.Now, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogTrace(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Trace, DateTime.Now, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogDebug(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Debug, DateTime.Now, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogInfo(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Info, DateTime.Now, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogWarning(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Warning, DateTime.Now, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public static void LogError(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Error, DateTime.Now, message);
        }
    }
}