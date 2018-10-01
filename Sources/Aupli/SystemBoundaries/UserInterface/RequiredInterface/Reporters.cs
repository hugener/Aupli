// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reporters.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.RequiredInterface
{
    using Aupli.SystemBoundaries.Pi.Display;
    using Aupli.SystemBoundaries.Pi.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Input;
    using Aupli.SystemBoundaries.UserInterface.Player.RequiredInterface;
    using Aupli.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Contains reporters for the user interface module.
    /// </summary>
    public class Reporters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reporters" /> class.
        /// </summary>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        /// <param name="inputManagerReporter">The input manager reporter.</param>
        /// <param name="interactionControllerReporter">The interaction controller reporter.</param>
        /// <param name="systemActivityAggregatorReporter">The system activity aggregator reporter.</param>
        /// <param name="idleControllerReporter">The idle controller reporter.</param>
        /// <param name="playerControllerReporter">The player controller reporter.</param>
        /// <param name="volumeControllerReporter">The volume controller reporter.</param>
        /// <param name="shutdownControllerReporter">The shutdown controller reporter.</param>
        /// <param name="viewNavigatorReporter">The view navigator reporter.</param>
        /// <param name="displayStateControllerReporter">The display backlight controller reporter.</param>
        public Reporters(
            ITextViewRendererReporter textViewRendererReporter,
            IInputManagerReporter inputManagerReporter,
            IInteractionControllerReporter interactionControllerReporter,
            ISystemActivityAggregatorReporter systemActivityAggregatorReporter,
            IIdleControllerReporter idleControllerReporter,
            IPlayerControllerReporter playerControllerReporter,
            IVolumeControllerReporter volumeControllerReporter,
            IShutdownControllerReporter shutdownControllerReporter,
            IViewNavigatorReporter viewNavigatorReporter,
            IDisplayStateControllerReporter displayStateControllerReporter)
        {
            this.TextViewRendererReporter = textViewRendererReporter;
            this.InputManagerReporter = inputManagerReporter;
            this.InteractionControllerReporter = interactionControllerReporter;
            this.SystemActivityAggregatorReporter = systemActivityAggregatorReporter;
            this.IdleControllerReporter = idleControllerReporter;
            this.PlayerControllerReporter = playerControllerReporter;
            this.VolumeControllerReporter = volumeControllerReporter;
            this.ShutdownControllerReporter = shutdownControllerReporter;
            this.ViewNavigatorReporter = viewNavigatorReporter;
            this.DisplayStateControllerReporter = displayStateControllerReporter;
        }

        /// <summary>
        /// Gets the text view renderer reporter.
        /// </summary>
        /// <value>
        /// The text view renderer reporter.
        /// </value>
        public ITextViewRendererReporter TextViewRendererReporter { get; }

        /// <summary>
        /// Gets the input manager reporter.
        /// </summary>
        /// <value>
        /// The input manager reporter.
        /// </value>
        public IInputManagerReporter InputManagerReporter { get; }

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
        public IIdleControllerReporter IdleControllerReporter { get; }

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