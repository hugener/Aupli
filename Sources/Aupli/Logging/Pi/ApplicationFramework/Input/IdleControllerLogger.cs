﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdleControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Pi.ApplicationFramework.Input
{
    using Serilog;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Logger for the IdleController.
    /// </summary>
    /// <seealso cref="IIdleControllerReporter" />
    public class IdleControllerLogger : IIdleControllerReporter
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleControllerLogger"/> class.
        /// </summary>
        /// <param name="logger">The log.</param>
        public IdleControllerLogger(ILogger logger)
        {
            this.log = logger.ForContext<IdleControllerLogger>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Started()
        {
            this.log.Debug(nameof(this.Started));
        }

        /// <summary>
        /// Inputs the activity.
        /// </summary>
        public void OnInputActivity()
        {
            this.log.Verbose(nameof(this.OnInputActivity));
        }

        /// <summary>
        /// Activateds this instance.
        /// </summary>
        public void Activated()
        {
            this.log.Debug(nameof(this.Activated));
        }

        /// <summary>
        /// Systems the activity.
        /// </summary>
        public void OnSystemActivity()
        {
            this.log.Verbose(nameof(this.OnSystemActivity));
        }

        /// <summary>
        /// Inputs the idle.
        /// </summary>
        public void OnInputIdle()
        {
            this.log.Debug(nameof(this.OnInputIdle));
        }

        /// <summary>
        /// Systems the idle.
        /// </summary>
        public void OnSystemIdle()
        {
            this.log.Debug(nameof(this.OnSystemIdle));
        }
    }
}