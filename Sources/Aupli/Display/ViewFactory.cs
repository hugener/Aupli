// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Display
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.Lifecycle;
    using Aupli.Menu;
    using Aupli.OperationSystem;
    using Aupli.Player;
    using Aupli.Volume;
    using Sundew.Base.Threading;

    /// <summary>
    /// Factory for creating views.
    /// </summary>
    public class ViewFactory
    {
        private readonly Lazy<StartUpTextView> startUpView;

        private readonly Lazy<PlayerTextView> playerView;

        private readonly Lazy<MenuTextView> menuView;

        private readonly AsyncLazy<VolumeTextView> volumeView;

        private readonly Lazy<ShutdownTextView> shutdownView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewFactory"/> class.
        /// </summary>
        /// <param name="controllerFactory">The controller factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="startupConfiguration">The startup configuration.</param>
        public ViewFactory(ControllerFactory controllerFactory, ConnectionFactory connectionFactory, ILifecycleConfiguration startupConfiguration)
        {
            this.startUpView = new Lazy<StartUpTextView>(() => new StartUpTextView(startupConfiguration));
            this.playerView = new Lazy<PlayerTextView>(() => new PlayerTextView(connectionFactory.MusicPlayer));
            this.menuView = new Lazy<MenuTextView>(() => new MenuTextView(new NetworkDeviceProvider(), controllerFactory.MenuController));
            this.volumeView = new AsyncLazy<VolumeTextView>(async () => new VolumeTextView(await controllerFactory.GetVolumeControllerAsync()), LazyThreadSafetyMode.ExecutionAndPublication);
            this.shutdownView = new Lazy<ShutdownTextView>(() => new ShutdownTextView(startupConfiguration));
        }

        /// <summary>
        /// Gets the start up textView.
        /// </summary>
        /// <value>
        /// The start up textView.
        /// </value>
        public StartUpTextView StartUpTextView => this.startUpView.Value;

        /// <summary>
        /// Gets the player textView.
        /// </summary>
        /// <value>
        /// The player textView.
        /// </value>
        public PlayerTextView PlayerTextView => this.playerView.Value;

        /// <summary>
        /// Gets the shutdown textView.
        /// </summary>
        /// <value>
        /// The shutdown textView.
        /// </value>
        public ShutdownTextView ShutdownTextView => this.shutdownView.Value;

        /// <summary>
        /// Gets the menu textView.
        /// </summary>
        /// <value>
        /// The menu textView.
        /// </value>
        public MenuTextView MenuTextView => this.menuView.Value;

        /// <summary>
        /// Gets the volume textView.
        /// </summary>
        /// <value>
        /// The volume textView.
        /// </value>
        public async Task<VolumeTextView> GetVolumeTextViewAsync()
        {
            return await this.volumeView;
        }
    }
}