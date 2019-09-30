// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmplifierLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.Pi.Amplifier
{
    using Aupli.SystemBoundaries.Pi.Amplifier.Ari;
    using global::Serilog;
    using global::Serilog.Events;
    using global::Sundew.Base;
    using global::Sundew.Base.Numeric;

    /// <summary>
    /// Logger reporter for the amplifier.
    /// </summary>
    /// <seealso cref="IAmplifierReporter" />
    public class AmplifierLogger : IAmplifierReporter
    {
        private readonly LogEventLevel logEventLevel;
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logEventLevel">The log event level.</param>
        public AmplifierLogger(ILogger logger, LogEventLevel logEventLevel)
        {
            this.logEventLevel = logEventLevel;
            this.logger = logger.ForContext<AmplifierLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.logger = this.logger.ForContext(source.AsType());
        }

        /// <summary>
        /// Changes the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        public void ChangeMute(bool isMuted)
        {
            this.logger.Write(this.logEventLevel, "Changed mute: {isMuted}", isMuted);
        }

        /// <summary>
        /// Changes the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        public void ChangeVolume(Percentage volume)
        {
            this.logger.Write(this.logEventLevel, "Changed volume: {volume}", volume);
        }
    }
}