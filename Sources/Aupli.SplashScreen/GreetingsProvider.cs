// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GreetingsProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SplashScreen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Sundew.Base.Collections;

    public static class GreetingsProvider
    {
        private const string LastGreetingPath = "last-greeting.val";
        private const string GreetingsPath = "greetings.csv";

        public static async Task<string> GetGreetingAsync()
        {
            var greetings = await GetGreetingsAsync();
            var lastGreeting = await GetLastGreetingAsync();
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

        public static async Task SaveLastGreetingAsync(string greeting)
        {
            await File.WriteAllTextAsync(LastGreetingPath, greeting);
        }

        private static async Task<IReadOnlyList<string>> GetGreetingsAsync()
        {
            var fileContent = await File.ReadAllTextAsync(GreetingsPath);
            return fileContent.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }

        private static async Task<string> GetLastGreetingAsync()
        {
            return await File.ReadAllTextAsync(LastGreetingPath);
        }
    }
}