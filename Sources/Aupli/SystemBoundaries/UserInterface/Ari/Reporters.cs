// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reporters.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Ari
{
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.UserInterface.Display.Ari;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Input.Ari;
    using Aupli.SystemBoundaries.UserInterface.Player.Ari;
    using Aupli.SystemBoundaries.UserInterface.Shutdown.Ari;
    using Aupli.SystemBoundaries.UserInterface.Volume.Ari;
    using Sundew.Pi.ApplicationFramework.Input;

    /// <summary>
    /// Contains reporters for the user interface module.
    /// </summary>
    public class Reporters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reporters" /> class.
        /// </summary>
        /// <param name="interactionControllerReporter">The interaction controller reporter.</param>
        /// <param name="systemActivityAggregatorReporter">The system activity aggregator reporter.</param>
        /// <param name="idleMonitorReporter">The idle controller reporter.</param>
        /// <param name="playerControllerReporter">The player controller reporter.</param>
        /// <param name="volumeControllerReporter">The volume controller reporter.</param>
        /// <param name="shutdownControllerReporter">The shutdown controller reporter.</param>
        /// <param name="viewNavigatorReporter">The view navigator reporter.</param>
        /// <param name="displayStateControllerReporter">The display backlight controller reporter.</param>
        public Reporters(
            IInteractionControllerReporter interactionControllerReporter,
            ISystemActivityAggregatorReporter systemActivityAggregatorReporter,
            IIdleMonitorReporter idleMonitorReporter,
            IPlayerControllerReporter playerControllerReporter,
            IVolumeControllerReporter volumeControllerReporter,
            IShutdownControllerReporter shutdownControllerReporter,
            IViewNavigatorReporter viewNavigatorReporter,
            IDisplayStateControllerReporter displayStateControllerReporter)
        {
            this.InteractionControllerReporter = interactionControllerReporter;
            this.SystemActivityAggregatorReporter = systemActivityAggregatorReporter;
            this.IdleMonitorReporter = idleMonitorReporter;
            this.PlayerControllerReporter = playerControllerReporter;
            this.VolumeControllerReporter = volumeControllerReporter;
            this.ShutdownControllerReporter = shutdownControllerReporter;
            this.ViewNavigatorReporter = viewNavigatorReporter;
            this.DisplayStateControllerReporter = displayStateControllerReporter;
        }

        /// <summary>
        /// Gets the interaction controller reporter.
        /// </summary>
        /// <value>
        /// The interaction controller reporter.
        /// </value>
        public IInteractionControllerReporter InteractionControllerReporter { get; }

        /// <summary>
        /// Gets the system activity aggregator reporter.
        /// </summary>
        /// <value>
        /// The system activity aggregator reporter.
        /// </value>
        public ISystemActivityAggregatorReporter SystemActivityAggregatorReporter { get; }

        /// <summary>
        /// Gets the idle controller reporter.
        /// </summary>
        /// <value>
        /// The idle controller reporter.
        /// </value>
        public IIdleMonitorReporter IdleMonitorReporter { get; }

        /// <summary>
        /// Gets the player controller reporter.
        /// </summary>
        /// <value>
        /// The player controller reporter.
        /// </value>
        public IPlayerControllerReporter PlayerControllerReporter { get; }

        /// <summary>
        /// Gets the volume controller reporter.
        /// </summary>
        /// <value>
        /// The volume controller reporter.
        /// </value>
        public IVolumeControllerReporter VolumeControllerReporter { get; }

        /// <summary>
        /// Gets the shutdown controller reporter.
        /// </summary>
        /// <value>
        /// The shutdown controller reporter.
        /// </value>
        public IShutdownControllerReporter ShutdownControllerReporter { get; }

        /// <summary>
        /// Gets the view navigator reporter.
        /// </summary>
        /// <value>
        /// The view navigator reporter.
        /// </value>
        public IViewNavigatorReporter ViewNavigatorReporter { get; }

        /// <summary>
        /// Gets the display backlight controller reporter.
        /// </summary>
        /// <value>
        /// The display backlight controller reporter.
        /// </value>
        public IDisplayStateControllerReporter DisplayStateControllerReporter { get; }
    }
}