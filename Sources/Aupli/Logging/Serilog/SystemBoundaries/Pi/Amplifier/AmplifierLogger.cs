// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmplifierLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.Pi.Amplifier
{
    using System;
    using System.Reflection;
    using Aupli.SystemBoundaries.Pi.Amplifier.Ari;
    using global::Pi.IO.InterIntegratedCircuit;
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
        private ILogger amplifierLogger;
        private ILogger ic2DeviceConnectionLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="logEventLevel">The log event level.</param>
        public AmplifierLogger(ILogger logger, LogEventLevel logEventLevel)
        {
            this.logEventLevel = logEventLevel;
            this.ic2DeviceConnectionLogger = this.amplifierLogger = logger.ForContext<AmplifierLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            if (target == typeof(II2cDeviceConnectionReporter))
            {
                this.ic2DeviceConnectionLogger = this.ic2DeviceConnectionLogger.ForContext(source.AsType());
            }
            else if (target == typeof(IAmplifierReporter))
            {
                this.amplifierLogger = this.amplifierLogger.ForContext(source.AsType());
            }
        }

        /// <summary>
        /// Changes the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        public void ChangeMute(bool isMuted)
        {
            this.amplifierLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{isMuted}}", isMuted);
        }

        /// <summary>
        /// Changes the volume.
        /// </summary>
        /// <param name="volume">The volume.</param>
        public void ChangeVolume(Percentage volume)
        {
            this.amplifierLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{volume}}", volume);
        }

        /// <summary>
        /// Connects the specified device address.
        /// </summary>
        /// <param name="deviceAddress">The device address.</param>
        public void Connect(int deviceAddress)
        {
            this.ic2DeviceConnectionLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{deviceAddress}}", deviceAddress);
        }

        /// <summary>
        /// Reads the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void Read(Span<byte> values)
        {
            this.ic2DeviceConnectionLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{values}}", values.ToString());
        }

        /// <summary>
        /// Wrotes the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void Wrote(Span<byte> values)
        {
            this.ic2DeviceConnectionLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{values}}", values.ToString());
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="values">The values.</param>
        public void WriteError(Exception exception, Span<byte> values)
        {
            this.ic2DeviceConnectionLogger.Error(exception, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{values}}", values.ToString());
        }

        /// <summary>
        /// Disposeds this instance.
        /// </summary>
        public void Disposed()
        {
            this.ic2DeviceConnectionLogger.Write(this.logEventLevel, $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}");
        }
    }
}