// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayStateControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Display
{
    using System;
    using System.Reflection;
    using Aupli.SystemBoundaries.UserInterface.Display.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IDisplayStateControllerReporter"/>.
    /// </summary>
    /// <seealso cref="IDisplayStateControllerReporter" />
    public class DisplayStateControllerLogger : IDisplayStateControllerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayStateControllerLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public DisplayStateControllerLogger(ILogger log)
        {
            this.log = log.ForContext<DisplayStateControllerLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Enableds the backlight.
        /// </summary>
        public void EnabledDisplay()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Disableds the backlight.
        /// </summary>
        public void DisabledDisplay()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }
    }
}