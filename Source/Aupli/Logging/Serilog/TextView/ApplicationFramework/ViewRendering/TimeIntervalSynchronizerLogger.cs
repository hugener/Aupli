// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeIntervalSynchronizerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.TextView.ApplicationFramework.ViewRendering
{
    using System;
    using System.Reflection;
    using global::Serilog;
    using Sundew.Base;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>Serilog implementation of <see cref="ITimeIntervalSynchronizerReporter"/>.</summary>
    /// <seealso cref="Sundew.TextView.ApplicationFramework.TextViewRendering.ITimeIntervalSynchronizerReporter" />
    public class TimeIntervalSynchronizerLogger : ITimeIntervalSynchronizerReporter
    {
        private ILogger logger;

        /// <summary>Initializes a new instance of the <see cref="TimeIntervalSynchronizerLogger"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public TimeIntervalSynchronizerLogger(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void SetSource(Type target, object source)
        {
            this.logger = this.logger.ForContext(source.AsType());
        }

        /// <summary>Delayeds the by.</summary>
        /// <param name="delay">The delay.</param>
        public void DelayedBy(TimeSpan delay)
        {
            this.logger.Verbose($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{Delay}}", delay);
        }

        /// <summary>Delays the not needed.</summary>
        /// <param name="elapsed">The elapsed.</param>
        public void DelayNotNeeded(TimeSpan elapsed)
        {
            this.logger.Verbose($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{Elapsed}}", elapsed);
        }

        /// <summary>Wases the aborted.</summary>
        public void WasAborted()
        {
            this.logger.Verbose($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}");
        }

        /// <summary>Intervals the changed.</summary>
        /// <param name="interval">The interval.</param>
        public void IntervalChanged(TimeSpan interval)
        {
            this.logger.Verbose($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{Interval}}", interval);
        }
    }
}