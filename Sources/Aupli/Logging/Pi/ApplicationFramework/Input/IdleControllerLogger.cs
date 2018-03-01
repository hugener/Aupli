// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdleControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Pi.ApplicationFramework.Input
{
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Logger for the IdleController.
    /// </summary>
    /// <seealso cref="IIdleControllerObserver" />
    public class IdleControllerLogger : IIdleControllerObserver
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleControllerLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public IdleControllerLogger(ILog log)
        {
            this.logger = log.GetCategorizedLogger<IdleControllerLogger>(true);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Started()
        {
            this.logger.LogDebug(nameof(this.Started));
        }

        /// <summary>
        /// Inputs the activity.
        /// </summary>
        public void OnInputActivity()
        {
            this.logger.LogTrace(nameof(this.OnInputActivity));
        }

        /// <summary>
        /// Activateds this instance.
        /// </summary>
        public void Activated()
        {
            this.logger.LogDebug(nameof(this.Activated));
        }

        /// <summary>
        /// Systems the activity.
        /// </summary>
        public void OnSystemActivity()
        {
            this.logger.LogTrace(nameof(this.OnSystemActivity));
        }

        /// <summary>
        /// Inputs the idle.
        /// </summary>
        public void OnInputIdle()
        {
            this.logger.LogDebug(nameof(this.OnInputIdle));
        }

        /// <summary>
        /// Systems the idle.
        /// </summary>
        public void OnSystemIdle()
        {
            this.logger.LogDebug(nameof(this.OnSystemIdle));
        }
    }
}