// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameTextFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Startup
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository for getting the name.
    /// </summary>
    public class NameTextFileRepository
    {
        private readonly string namePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameTextFileRepository"/> class.
        /// </summary>
        /// <param name="namePath">The name path.</param>
        public NameTextFileRepository(string namePath)
        {
            this.namePath = namePath;
        }

        /// <summary>
        /// Gets the name asynchronous.
        /// </summary>
        /// <returns>The name in an async task.</returns>
        public Task<string> GetNameAsync()
        {
            return File.ReadAllTextAsync(this.namePath);
        }
    }
}