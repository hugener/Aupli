// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Volume
{
    using System;
    using System.Collections.Generic;
    using Aupli.ApplicationServices.Volume.Api;
    using Sundew.Base.Collections;
    using Sundew.Base.Numeric;
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for displaying the current volume.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class VolumeTextView : ITextView
    {
        private readonly VolumeController volumeController;
        private readonly IVolumeService volumeService;
        private Percentage volume;
        private bool isMuted;
        private IInvalidater invalidater;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeTextView" /> class.
        /// </summary>
        /// <param name="volumeController">The volume controller.</param>
        /// <param name="volumeService">The volume service.</param>
        public VolumeTextView(VolumeController volumeController, IVolumeService volumeService)
        {
            this.volumeController = volumeController;
            this.volumeService = volumeService;
            this.volumeService.VolumeChanged += this.OnVolumeServiceVolumeChanged;
            this.volume = volumeService.Volume;
        }

        /// <inheritdoc />
        public IEnumerable<object> InputTargets => this.volumeController.ToEnumerable();

        /// <inheritdoc />
        public void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.invalidater = invalidater;
        }

        /// <summary>
        /// Renders the specified textView information.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        public void Render(IRenderContext renderContext)
        {
            renderContext.Home();
            var line1 = $"Volume: {this.volume.ToString(0),5}{' '.Repeat(renderContext.Size.Width - 5 + 8)}";
            if (this.isMuted)
            {
                line1 = "Muted".LimitAndPadRight(renderContext.Size.Width - 5, ' ');
            }

            renderContext.WriteLine(line1);
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }

        private void OnVolumeServiceVolumeChanged(object sender, EventArgs e)
        {
            if (this.isMuted != this.volumeService.IsMuted || this.volume != this.volumeService.Volume)
            {
                this.volume = this.volumeService.Volume;
                this.isMuted = this.volumeService.IsMuted;
                this.invalidater.Invalidate();
            }
        }
    }
}