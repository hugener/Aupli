// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    /// <summary>
    /// Extends <see cref="ILog"/> with easy to use methods.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Gets the categorized logger.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="log">The log.</param>
        /// <param name="useNameOnly">if set to <c>true</c> [name only].</param>
        /// <returns>
        /// A new <see cref="ILogger" />.
        /// </returns>
        public static ILogger GetCategorizedLogger<TType>(this ILog log, bool useNameOnly = false)
        {
            return log.GetCategorizedLogger(typeof(TType), useNameOnly);
        }

        /// <summary>
        /// Gets the categorized logger.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="target">The target.</param>
        /// <param name="useNameOnly">if set to <c>true</c> [name only].</param>
        /// <returns>
        /// A new <see cref="ILogger" />.
        /// </returns>
        public static ILogger GetCategorizedLogger(this ILog log, object target, bool useNameOnly = false)
        {
            return log.GetCategorizedLogger(target, useNameOnly);
        }

        /// <summary>
        /// Gets the categorized logger.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="type">The type.</param>
        /// <param name="useNameOnly">if set to <c>true</c> [name only].</param>
        /// <returns>
        /// A new <see cref="ILogger" />.
        /// </returns>
        public static ILogger GetCategorizedLogger(this ILog log, Type type, bool useNameOnly = false)
        {
            return log.GetCategorizedLogger(useNameOnly ? type.Name : type.FullName);
        }
    }
}