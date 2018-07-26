// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Max9744Amplifier.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Amplifier
{
    using Aupli.ApplicationServices.RequiredInterface.Amplifier;
    using Sundew.Base.Numeric;
    using Sundew.Pi.IO.Devices.Amplifiers.Max9744;

    /// <summary>
    /// Implements <see cref="IAmplifier"/> using a <see cref="Max9744Device"/>.
    /// </summary>
    /// <seealso cref="IAmplifier" />
    public class Max9744Amplifier : IAmplifier
    {
        private readonly Max9744Device max9744Device;
        private readonly IAmplifierReporter max9744AmplifierReporter;
        private readonly Range<byte> volumeRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="Max9744Amplifier" /> class.
        /// </summary>
        /// <param name="max9744Device">The max9744 device.</param>
        /// <param name="max9744AmplifierReporter">The max9744 amplifier reporter.</param>
        public Max9744Amplifier(Max9744Device max9744Device, IAmplifierReporter max9744AmplifierReporter = null)
        {
            this.max9744Device = max9744Device;
            this.max9744AmplifierReporter = max9744AmplifierReporter;
            this.max9744AmplifierReporter?.SetSource(this);
            this.volumeRange = new Range<byte>(
                (byte)(max9744Device.VolumeRange.Min + 10),
                (byte)(max9744Device.VolumeRange.Max - 30));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is muted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is muted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMuted
        {
            get => this.max9744Device.IsMuted;
            set
            {
                this.max9744Device.SetMuteState(value);
                this.max9744AmplifierReporter?.ChangeMute(value);
            }
        }

        /// <summary>
        /// Sets the volume.
        /// </summary>
        /// <param name="volume">The volume value.</param>
        public void SetVolume(Percentage volume)
        {
            var absoluteVolume = (byte)(((this.volumeRange.Max - this.volumeRange.Min) * volume) + this.volumeRange.Min);
            this.max9744Device.SetVolume(absoluteVolume);
            this.max9744AmplifierReporter?.ChangeVolume(volume);
        }
    }
}