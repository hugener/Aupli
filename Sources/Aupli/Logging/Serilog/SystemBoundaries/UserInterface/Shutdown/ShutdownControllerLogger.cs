// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShutdownControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Shutdown
{
    using Aupli.SystemBoundaries.UserInterface.Shutdown.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IShutdownControllerReporter"/>.
    /// </summary>
    /// <seealso cref="IShutdownControllerReporter" />
    public class ShutdownControllerLogger : IShutdownControllerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShutdownControllerLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ShutdownControllerLogger(ILogger logger)
        {
            this.log = logger.ForContext<ShutdownControllerLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Remotes the pi shutdown.
        /// </summary>
        public void RemotePiShutdown()
        {
            this.log.Debug(nameof(this.RemotePiShutdown));
        }

        /// <summary>
        /// Shuttings down aupli.
        /// </summary>
        public void ShuttingDownAupli()
        {
            this.log.Information(nameof(this.ShuttingDownAupli));
        }

        /// <summary>
        /// Closings the aupli.
        /// </summary>
        public void ClosingAupli()
        {
            this.log.Information(nameof(this.ClosingAupli));
        }

        /// <summary>
        /// Systems the idle shutdown.
        /// </summary>
        public void SystemIdleShutdown()
        {
            this.log.Debug(nameof(this.SystemIdleShutdown));
        }

        /// <summary>
        /// Shutdowns the by control c.
        /// </summary>
        public void ShutdownByCtrlC()
        {
            this.log.Information(nameof(this.ShutdownByCtrlC));
        }
    }
}