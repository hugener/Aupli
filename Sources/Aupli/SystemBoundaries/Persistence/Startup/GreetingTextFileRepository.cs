// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GreetingTextFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Startup
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Sundew.Base.Collections;

    /// <summary>
    /// Text file based implementation of <see cref="IGreetingProvider"/>.
    /// </summary>
    /// <seealso cref="IGreetingProvider" />
    public class GreetingTextFileRepository : IGreetingProvider
    {
        private readonly string greetingsPath;
        private readonly string lastGreetingPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="GreetingTextFileRepository"/> class.
        /// </summary>
        /// <param name="greetingsPath">The greetings path.</param>
        /// <param name="lastGreetingPath">The last greeting path.</param>
        public GreetingTextFileRepository(string greetingsPath, string lastGreetingPath)
        {
            this.greetingsPath = greetingsPath;
            this.lastGreetingPath = lastGreetingPath;
        }

        /// <summary>
        /// Gets the greeting asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task<string> GetGreetingAsync()
        {
            var greetings = await this.GetGreetingsAsync();
            var lastGreeting = await this.GetLastGreetingAsync();
            if (greetings == null || !greetings.Any())
            {
                return "Hello";
            }

            var lastGreetingIndex = greetings.IndexOf(x => x == lastGreeting);
            if (lastGreetingIndex == -1)
            {
                lastGreetingIndex = greetings.Count - 1;
            }

            lastGreetingIndex++;
            return greetings[lastGreetingIndex % greetings.Count];
        }

        /// <summary>
        /// Saves the last greeting asynchronous.
        /// </summary>
        /// <param name="greeting">The greeting.</param>
        /// <returns>An async task.</returns>
        public async Task SaveLastGreetingAsync(string greeting)
        {
            await File.WriteAllTextAsync(this.lastGreetingPath, greeting);
        }

        private async Task<IReadOnlyList<string>> GetGreetingsAsync()
        {
            var fileContent = await File.ReadAllTextAsync(this.greetingsPath);
            return fileContent.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        private async Task<string> GetLastGreetingAsync()
        {
            return await File.ReadAllTextAsync(this.lastGreetingPath);
        }
    }
}