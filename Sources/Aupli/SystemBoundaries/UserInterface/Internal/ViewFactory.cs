// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Internal
{
    using System;
    using System.Threading;
    using Aupli.ApplicationServices.Volume.Api;
    using Aupli.SystemBoundaries.Mpc;
    using Aupli.SystemBoundaries.OperationSystem.Linux;
    using Aupli.SystemBoundaries.Shared.Lifecycle;
    using Aupli.SystemBoundaries.UserInterface.Menu;
    using Aupli.SystemBoundaries.UserInterface.Player;
    using Aupli.SystemBoundaries.UserInterface.Shutdown;
    using Aupli.SystemBoundaries.UserInterface.Volume;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Factory for creating views.
    /// </summary>
    internal class ViewFactory
    {
        private readonly Lazy<PlayerTextView> playerView;

        private readonly Lazy<MenuTextView> menuView;

        private readonly Lazy<VolumeTextView> volumeView;

        private readonly Lazy<ShutdownTextView> shutdownView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewFactory" /> class.
        /// </summary>
        /// <param name="musicPlayer">The music player.</param>
        /// <param name="playerController">The player controller.</param>
        /// <param name="volumeController">The volume controller.</param>
        /// <param name="volumeService">The volume service.</param>
        /// <param name="menuController">The menu controller.</param>
        /// <param name="lifecycleConfiguration">The lifecycle configuration.</param>
        public ViewFactory(IMusicPlayer musicPlayer, PlayerController playerController, VolumeController volumeController, IVolumeService volumeService, MenuController menuController, ILifecycleConfiguration lifecycleConfiguration)
        {
            this.playerView = new Lazy<PlayerTextView>(() => new PlayerTextView(playerController, musicPlayer, volumeService), LazyThreadSafetyMode.ExecutionAndPublication);
            this.menuView = new Lazy<MenuTextView>(() => new MenuTextView(new NetworkDeviceInfoProvider(), menuController));
            this.volumeView = new Lazy<VolumeTextView>(() => new VolumeTextView(volumeController, volumeService), LazyThreadSafetyMode.ExecutionAndPublication);
            this.shutdownView = new Lazy<ShutdownTextView>(() => new ShutdownTextView(lifecycleConfiguration));
        }

        /// <summary>
        /// Gets the shutdown textView.
        /// </summary>
        /// <value>
        /// The shutdown textView.
        /// </value>
        public ITextView ShutdownTextView => this.shutdownView.Value;

        /// <summary>
        /// Gets the menu textView.
        /// </summary>
        /// <value>
        /// The menu textView.
        /// </value>
        public ITextView MenuTextView => this.menuView.Value;

        /// <summary>
        /// Gets the player textView.
        /// </summary>
        /// <value>
        /// The player textView.
        /// </value>
        public ITextView PlayerTextView => this.playerView.Value;

        /// <summary>
        /// Gets the volume textView.
        /// </summary>
        /// <value>
        /// The volume textView.
        /// </value>
        public ITextView VolumeTextView => this.volumeView.Value;
    }
}