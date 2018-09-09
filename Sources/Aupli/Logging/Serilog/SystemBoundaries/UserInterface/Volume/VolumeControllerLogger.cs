// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Volume
{
    using Aupli.SystemBoundaries.Shared.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IVolumeControllerReporter"/>.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.UserInterface.Volume.IVolumeControllerReporter" />
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
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Keys the input.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        public void KeyInput(KeyInput keyInput)
        {
            this.log.Debug($"{nameof(this.KeyInput)} {{{nameof(keyInput)}}}", keyInput);
        }
    }
}