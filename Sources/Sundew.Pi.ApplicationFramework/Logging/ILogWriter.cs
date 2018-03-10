// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing a log writer.
    /// </summary>
    public interface ILogWriter : IDisposable
    {
        /// <summary>
        /// Writes the specified log message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An async task.</returns>
        Task WriteAsync(string message);

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();
    }
}