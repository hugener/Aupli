// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleLogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Write writer that outputs to the console.
    /// </summary>
    /// <seealso cref="Sundew.Pi.ApplicationFramework.Logging.ILogWriter" />
    public class ConsoleLogWriter : ILogWriter
    {
        /// <summary>
        /// Logs the specified message to the console.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A completed task.</returns>
        public Task WriteAsync(string message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Initialize()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}