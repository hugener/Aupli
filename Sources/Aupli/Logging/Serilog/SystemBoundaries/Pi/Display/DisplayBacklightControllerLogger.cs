// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayBacklightControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.Pi.Display
{
    using Aupli.SystemBoundaries.Pi.Display;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IDisplayBacklightControllerReporter"/>.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Pi.Display.IDisplayBacklightControllerReporter" />
    public class DisplayBacklightControllerLogger : IDisplayBacklightControllerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayBacklightControllerLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public DisplayBacklightControllerLogger(ILogger log)
        {
            this.log = log.ForContext<DisplayBacklightControllerLogger>();
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
        /// Enableds the backlight.
        /// </summary>
        public void EnabledBacklight()
        {
            this.log.Debug(nameof(this.EnabledBacklight));
        }

        /// <summary>
        /// Disableds the backlight.
        /// </summary>
        public void DisabledBacklight()
        {
            this.log.Debug(nameof(this.DisabledBacklight));
        }
    }
}