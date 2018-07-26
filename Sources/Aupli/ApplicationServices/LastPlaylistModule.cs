// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LastPlaylistModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices
{
    using Aupli.ApplicationServices.Player;
    using Aupli.ApplicationServices.RequiredInterface.Playlist;
    using Aupli.DomainServices.RequiredInterface.Playlist;

    /// <summary>
    /// The last playlist module.
    /// </summary>
    public class LastPlaylistModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LastPlaylistModule"/> class.
        /// </summary>
        /// <param name="lastPlaylistRepository">The last playlist repository.</param>
        public LastPlaylistModule(ILastPlaylistRepository lastPlaylistRepository)
        {
            this.LastPlaylistChangeHandler = new LastPlaylistChangeHandler(lastPlaylistRepository);
        }

        /// <summary>
        /// Gets the last playlist change handler.
        /// </summary>
        /// <value>
        /// The last playlist change handler.
        /// </value>
        public ILastPlaylistChangeHandler LastPlaylistChangeHandler { get; }
    }
}