// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    /// <summary>
    /// Interface for implementing a logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="message">The message.</param>
        void Log(LogLevel logLevel, DateTime dateTime, string message);
    }
}