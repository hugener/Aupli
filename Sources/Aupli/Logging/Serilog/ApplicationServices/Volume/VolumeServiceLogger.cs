// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeServiceLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.ApplicationServices.Volume
{
    using Aupli.ApplicationServices.Volume.Ari;
    using global::Serilog;
    using global::Sundew.Base;
    using global::Sundew.Base.Numeric;

    /// <summary>
    /// Logger for <see cref="IVolumeServiceReporter"/>.
    /// </summary>
    /// <seealso cref="IVolumeServiceReporter" />
    public class VolumeServiceLogger : IVolumeServiceReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeServiceLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public VolumeServiceLogger(ILogger logger)
        {
            this.log = logger.ForContext<VolumeServiceLogger>();
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
        /// Changes the volume.
        /// </summary>
        /// <param name="newVolumePercentage">The new volume percentage.</param>
        public void ChangeVolume(Percentage newVolumePercentage)
        {
            this.log.Debug($"{nameof(this.ChangeVolume)} {{{nameof(newVolumePercentage)}}}", newVolumePercentage);
        }

        /// <summary>
        /// Changes the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [new is muted].</param>
        public void ChangeMute(bool isMuted)
        {
            this.log.Debug($"{nameof(this.ChangeMute)} {{{nameof(isMuted)}}}", isMuted);
        }
    }
}