// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileLogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System.IO;

    /// <summary>
    /// Implementation of <see cref="ILog"/> that logs to a file.
    /// </summary>
    /// <seealso cref="ILog" />
    public class FileLogWriter : ILogWriter
    {
        private readonly string logPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogWriter"/> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        public FileLogWriter(string logPath)
        {
            this.logPath = logPath;
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Write(string message)
        {
            File.AppendAllText(this.logPath, message + "\r\n");
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}