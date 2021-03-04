// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGreetingProvider.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Bridges.Lifecycle
{
    using System.Threading.Tasks;

    /// <summary>
    /// Bridge interface for providing greetings and the last greeting.
    /// </summary>
    public interface IGreetingProvider
    {
        /// <summary>
        /// Gets the greeting asynchronous.
        /// </summary>
        /// <returns>An async task with the last greeting.</returns>
        Task<string> GetGreetingAsync();

        /// <summary>
        /// Saves the last greeting asynchronous.
        /// </summary>
        /// <param name="greeting">The greeting.</param>
        /// <returns>An async task.</returns>
        Task SaveLastGreetingAsync(string greeting);
    }
}