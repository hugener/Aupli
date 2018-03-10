// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiLogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Sundew.Base.Collections;

    /// <summary>
    /// Write writer that outputs to multiple <see cref="ILogWriter"/>.
    /// </summary>
    /// <seealso cref="ILogWriter" />
    public class MultiLogWriter : ILogWriter
    {
        private readonly IEnumerable<ILogWriter> logWriters;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLogWriter"/> class.
        /// </summary>
        /// <param name="logWriters">The log writers.</param>
        public MultiLogWriter(IEnumerable<ILogWriter> logWriters)
        {
            this.logWriters = logWriters.ToList();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.logWriters.ForEach(x => x.Initialize());
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An async task.</returns>
        public async Task WriteAsync(string message)
        {
            await this.logWriters.ForEachAsync(async x => await x.WriteAsync(message));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.logWriters.ForEach(x => x.Dispose());
        }
    }
}