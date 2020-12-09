// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Volume
{
    using System;
    using System.Reflection;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Volume.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IVolumeControllerReporter"/>.
    /// </summary>
    /// <seealso cref="IVolumeControllerReporter" />
    public class VolumeControllerLogger : IVolumeControllerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeControllerLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public VolumeControllerLogger(ILogger logger)
        {
            this.log = logger.ForContext<VolumeControllerLogger>();
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
        /// Keys the input.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        public void KeyInput(KeyInput keyInput)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{{nameof(keyInput)}}}", keyInput);
        }
    }
}