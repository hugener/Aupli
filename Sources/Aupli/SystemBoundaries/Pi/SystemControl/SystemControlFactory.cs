// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemControlFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.SystemControl
{
    using Aupli.SystemBoundaries.Bridges.Shutdown;
    using Aupli.SystemBoundaries.Pi.SystemControl.Linux;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Base.Disposal;
    using Sundew.Pi.IO.Devices.PowerManagement.RemotePi;

    /// <summary>
    /// Factory for creating a remote PI device.
    /// </summary>
    public class SystemControlFactory : ISystemControlFactory
    {
        private readonly DisposingDictionary<ISystemControl> systemControls = new DisposingDictionary<ISystemControl>();

        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriverFactory">The gpio connection driver factory.</param>
        /// <returns>
        /// A <see cref="RemotePiDevice" />.
        /// </returns>
        public ISystemControl Create(IGpioConnectionDriverFactory gpioConnectionDriverFactory)
        {
            var remotePiDevice = new RemotePiDevice(ConnectorPin.P1Pin08, ConnectorPin.P1Pin10, new OperatingSystemControl(), default, gpioConnectionDriverFactory);
            return this.systemControls.Add(new ShutdownSystemControl(remotePiDevice), remotePiDevice);
        }

        /// <summary>
        /// Disposes the specified system control.
        /// </summary>
        /// <param name="systemControl">The system control.</param>
        public void Dispose(ISystemControl systemControl)
        {
            this.systemControls.Dispose(systemControl);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.systemControls.Dispose();
        }
    }
}