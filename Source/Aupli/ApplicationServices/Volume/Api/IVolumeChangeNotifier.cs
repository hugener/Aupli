// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeChangeNotifier.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Api
{
    using System;

    /// <summary>
    /// Interface for implementing a volume change notifier.
    /// </summary>
    public interface IVolumeChangeNotifier
    {
        /// <summary>
        /// Occurs when [volume changed].
        /// </summary>
        event EventHandler VolumeChanged;
    }
}