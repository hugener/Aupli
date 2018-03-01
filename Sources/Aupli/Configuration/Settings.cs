// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using Aupli.Lifecycle;
    using Aupli.Player;
    using Aupli.Volume;

    /// <inheritdoc />
    /// <summary>
    /// Contains configuration for the application lifecycle.
    /// </summary>
    /// <seealso cref="T:Aupli.Lifecycle.IStartupConfiguration" />
    public class Settings : ILifecycleConfiguration, IVolumeSettings, IPlayerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="lastGreeting">The last greeting.</param>
        /// <param name="idleTimeout">The idle timeout.</param>
        /// <param name="systemTimeout">The system timeout.</param>
        /// <param name="volume">The volume.</param>
        /// <param name="pin26Feature">The pin26 feature.</param>
        /// <param name="lastPlaylist">The last playlist.</param>
        public Settings(
            string name,
            string lastGreeting,
            TimeSpan idleTimeout,
            TimeSpan systemTimeout,
            double volume,
            Pin26Feature pin26Feature,
            string lastPlaylist)
        {
            this.Name = name;
            this.LastGreeting = lastGreeting;
            this.IdleTimeout = idleTimeout;
            this.SystemTimeout = systemTimeout;
            this.Volume = volume;
            this.Pin26Feature = pin26Feature;
            this.LastPlaylist = lastPlaylist;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the last greeting.
        /// </summary>
        /// <value>
        /// The last greeting.
        /// </value>
        public string LastGreeting { get; set; }

        /// <summary>
        /// Gets the idle timeout.
        /// </summary>
        /// <value>
        /// The idle timeout.
        /// </value>
        public TimeSpan IdleTimeout { get; }

        /// <summary>
        /// Gets the system timeout.
        /// </summary>
        /// <value>
        /// The system timeout.
        /// </value>
        public TimeSpan SystemTimeout { get; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public double Volume { get; set; }

        /// <summary>
        /// Gets the pin26 feature.
        /// </summary>
        /// <value>
        /// The pin26 feature.
        /// </value>
        public Pin26Feature Pin26Feature { get; }

        /// <summary>
        /// Gets or sets the last playlist.
        /// </summary>
        /// <value>
        /// The last playlist.
        /// </value>
        public string LastPlaylist { get; set; }
    }
}