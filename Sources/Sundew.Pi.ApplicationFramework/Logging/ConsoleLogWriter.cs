// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleLogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

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
        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}