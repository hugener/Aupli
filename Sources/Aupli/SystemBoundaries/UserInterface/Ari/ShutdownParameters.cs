// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownParameters.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Ari
{
    using System.Threading;

    /// <summary>
    /// Contains the shutdown parameters.
    /// </summary>
    /// <seealso cref="IShutdownParameters" />
    public class ShutdownParameters : IShutdownParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownParameters"/> class.
        /// </summary>
        /// <param name="allowShutdown">if set to <c>true</c> [allow shutdown].</param>
        /// <param name="shutdownCancellationTokenSource">The shutdown cancellation token source.</param>
        public ShutdownParameters(bool allowShutdown, CancellationTokenSource shutdownCancellationTokenSource)
        {
            this.AllowShutdown = allowShutdown;
            this.ShutdownCancellationTokenSource = shutdownCancellationTokenSource;
        }

        /// <summary>
        /// Gets a value indicating whether [allow shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow shutdown]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowShutdown { get; }

        /// <summary>
        /// Gets the shutdown cancellation token source.
        /// </summary>
        /// <value>
        /// The shutdown cancellation token source.
        /// </value>
        public CancellationTokenSource ShutdownCancellationTokenSource { get; }
    }
}