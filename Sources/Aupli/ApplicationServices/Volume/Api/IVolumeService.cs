// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVolumeService.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Volume.Api
{
    using System.Threading.Tasks;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Interface for the volume service.
    /// </summary>
    /// <seealso cref="IVolumeStatus" />
    /// <seealso cref="IVolumeChangeNotifier" />
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    public interface IVolumeService : IVolumeStatus, IVolumeChangeNotifier, IInitializable
    {
        /// <summary>
        /// Changes the volume asynchronous.
        /// </summary>
        /// <param name="isIncrementing">if set to <c>true</c> [is incrementing].</param>
        /// <returns>An async task.</returns>
        Task ChangeVolumeAsync(bool isIncrementing);

        /// <summary>
        /// Toggles the mute.
        /// </summary>
        /// <returns>An async task.</returns>
        Task ToggleMuteAsync();
    }
}