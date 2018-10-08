// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AmplifierFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.Amplifier
{
    using Aupli.ApplicationServices.Volume.Ari;
    using Aupli.SystemBoundaries.Pi.Amplifier.Api;
    using Aupli.SystemBoundaries.Pi.Amplifier.Ari;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Pi.IO.Devices.Amplifiers.Max9744;

    /// <summary>
    /// Factory for create Max9744 volume controls.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class AmplifierFactory : IAmplifierFactory
    {
        private readonly DisposingDictionary<IAmplifier> amplifiers = new DisposingDictionary<IAmplifier>();

        /// <summary>
        /// Creates the specified pin26 feature.
        /// </summary>
        /// <param name="amplifierReporter">The amplifier reporter.</param>
        /// <returns>
        /// The volume controls.
        /// </returns>
        public IAmplifier Create(IAmplifierReporter amplifierReporter)
        {
            var max9744Device = new Max9744Device(
                0x4b,
                ConnectorPin.P1Pin07,
                ConnectorPin.P1Pin11,
                ProcessorPin.Pin02,
                ProcessorPin.Pin03);
            max9744Device.SetMuteState(true);
            max9744Device.SetShutdownState(false);
            var amplifier = new Max9744Amplifier(max9744Device, amplifierReporter);
            return this.amplifiers.Add(amplifier, max9744Device);
        }

        /// <summary>
        /// Disposes the specified amplifier.
        /// </summary>
        /// <param name="amplifier">The amplifier.</param>
        public void Dispose(IAmplifier amplifier)
        {
            this.amplifiers.Dispose(amplifier);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.amplifiers.Dispose();
        }
    }
}