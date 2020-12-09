// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposableLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SundewBase
{
    using System;
    using System.Reflection;
    using global::Serilog;
    using Sundew.Base;
    using Sundew.Base.Disposal;

    /// <summary>Implements <see cref="IDisposableReporter"/> for Serilog.</summary>
    /// <seealso cref="IDisposableReporter" />
    public class DisposableLogger : IDisposableReporter
    {
        private ILogger logger;

        /// <summary>Initializes a new instance of the <see cref="DisposableLogger"/> class.</summary>
        /// <param name="logger">The logger.</param>
        public DisposableLogger(ILogger logger)
        {
            this.logger = logger.ForContext<DisposableLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.logger = this.logger.ForContext(source.AsType());
        }

        /// <summary>Called when [disposed].</summary>
        /// <param name="disposable">The disposable.</param>
        public void Disposed(object disposable)
        {
            this.logger.Verbose($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{Disposable}}", disposable);
        }
    }
}
