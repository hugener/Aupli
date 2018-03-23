// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeControls.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using Sundew.Pi.IO.Devices.Amplifiers.Max9744;
    using Sundew.Pi.IO.Devices.Buttons;

    /// <summary>
    /// Hardware controls for controlling volume.
    /// </summary>
    public class VolumeControls
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeControls" /> class.
        /// </summary>
        /// <param name="max9744Device">The max9744 connection.</param>
        /// <param name="headPhoneSwitchDevice">The head phone switch connection.</param>
        /// <param name="volumeAdjuster">The volume adjuster.</param>
        public VolumeControls(Max9744Device max9744Device, PullDownSwitchDevice headPhoneSwitchDevice, VolumeAdjuster volumeAdjuster)
        {
            this.Amplifier = max9744Device;
            this.HeadPhoneSwitch = headPhoneSwitchDevice;
            this.VolumeAdjuster = volumeAdjuster;
        }

        /// <summary>
        /// Gets the amplifier.
        /// </summary>
        /// <value>
        /// The amplifier.
        /// </value>
        public Max9744Device Amplifier { get; }

        /// <summary>
        /// Gets the head phone switch.
        /// </summary>
        /// <value>
        /// The head phone switch.
        /// </value>
        public PullDownSwitchDevice HeadPhoneSwitch { get; }

        /// <summary>
        /// Gets the volume adjuster.
        /// </summary>
        /// <value>
        /// The volume adjuster.
        /// </value>
        public VolumeAdjuster VolumeAdjuster { get; }
    }
}