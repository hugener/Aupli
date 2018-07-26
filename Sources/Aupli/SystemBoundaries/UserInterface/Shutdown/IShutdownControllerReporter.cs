// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShutdownControllerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Shutdown
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Reporter for <see cref="ShutdownController"/>.
    /// </summary>
    /// <seealso cref="Sundew.Base.Reporting.IReporter" />
    public interface IShutdownControllerReporter : IReporter
    {
        /// <summary>
        /// Remotes the pi shutdown.
        /// </summary>
        void RemotePiShutdown();

        /// <summary>
        /// Shuttings down aupli.
        /// </summary>
        void ShuttingDownAupli();

        /// <summary>
        /// Closings the aupli.
        /// </summary>
        void ClosingAupli();

        /// <summary>
        /// Systems the idle shutdown.
        /// </summary>
        void SystemIdleShutdown();
    }
}