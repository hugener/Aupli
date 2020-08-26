// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicControlModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.MusicControl
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.MusicControl;
    using Aupli.SystemBoundaries.MusicControl.Ari;
    using MpcNET;
    using Sundew.Base.Initialization;

    /// <summary>
    /// Module for the music player.
    /// </summary>
    /// <seealso cref="Sundew.Base.Initialization.IInitializable" />
    /// <seealso cref="System.IDisposable" />
    public class MusicControlModule : IInitializable, IDisposable
    {
        private readonly IMusicPlayerReporter? musicPlayerReporter;
        private IMpcConnection? mpcConnection;
        private IMusicPlayer? musicPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicControlModule"/> class.
        /// </summary>
        /// <param name="musicPlayerReporter">The music player reporter.</param>
        public MusicControlModule(IMusicPlayerReporter? musicPlayerReporter)
        {
            this.musicPlayerReporter = musicPlayerReporter;
        }

        /// <summary>
        /// Gets the music player.
        /// </summary>
        /// <value>
        /// The music player.
        /// </value>
        public IMusicPlayer MusicPlayer
        {
            get => this.musicPlayer ?? throw new NotInitializedException(this);
            private set => this.musicPlayer = value;
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns>An async task.</returns>
        public async ValueTask InitializeAsync()
        {
            this.mpcConnection = this.CreateMpcConnection(this.musicPlayerReporter);
            this.MusicPlayer = new MusicPlayer(this.mpcConnection, this.musicPlayerReporter);
            await this.mpcConnection.ConnectAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.MusicPlayer?.Dispose();
            this.mpcConnection?.Dispose();
        }

        /// <summary>
        /// Creates the MPC connection.
        /// </summary>
        /// <param name="mpcConnectionReporter">The music player logger.</param>
        /// <returns>A mpc connection.</returns>
        protected virtual IMpcConnection CreateMpcConnection(IMpcConnectionReporter? mpcConnectionReporter)
        {
            return new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600), mpcConnectionReporter);
        }
    }
}