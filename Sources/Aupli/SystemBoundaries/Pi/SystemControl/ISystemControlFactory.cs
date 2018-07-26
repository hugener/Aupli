// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemControlFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Pi.SystemControl
{
    using System;
    using Aupli.SystemBoundaries.Connectors.System;
    using global::Pi.IO.GeneralPurpose;
    using Sundew.Pi.IO.Devices.PowerManagement.RemotePi;

    /// <summary>
    /// Interface for implementing a sytem control factory.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ISystemControlFactory : IDisposable
    {
        /// <summary>
        /// Creates the specified gpio connection driver.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <returns>A <see cref="RemotePiDevice"/>.</returns>
        ISystemControl Create(IGpioConnectionDriver gpioConnectionDriver);

        /// <summary>
        /// Disposes the specified system control.
        /// </summary>
        /// <param name="systemControl">The system control.</param>
        void Dispose(ISystemControl systemControl);
    }
}