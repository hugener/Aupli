// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SplashScreen
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the name.
    /// </summary>
    public static class NameProvider
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>The name.</returns>
        public static async Task<string> GetNameAsync()
        {
            return await File.ReadAllTextAsync("name.val");
        }
    }
}