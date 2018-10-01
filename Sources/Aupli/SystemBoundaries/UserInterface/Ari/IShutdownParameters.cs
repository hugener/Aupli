// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShutdownParameters.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Ari
{
    using System.Threading;

    /// <summary>
    /// Interface for the shutdown parameters.
    /// </summary>
    public interface IShutdownParameters
    {
        /// <summary>
        /// Gets a value indicating whether [allow shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow shutdown]; otherwise, <c>false</c>.
        /// </value>
        bool AllowShutdown { get; }

        /// <summary>
        /// Gets the shutdown cancellation token source.
        /// </summary>
        /// <value>
        /// The shutdown cancellation token source.
        /// </value>
        CancellationTokenSource ShutdownCancellationTokenSource { get; }
    }
}