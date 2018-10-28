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
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume.Api;
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for displaying the current volume.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class VolumeTextView : ITextView
    {
        private readonly IVolumeService volumeService;
        private IInvalidater invalidater;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeTextView" /> class.
        /// </summary>
        /// <param name="volumeService">The volume service.</param>
        public VolumeTextView(IVolumeService volumeService)
        {
            this.volumeService = volumeService;
            this.volumeService.VolumeChanged += this.OnVolumeServiceVolumeChanged;
        }

        /// <inheritdoc />
        public IEnumerable<object> InputTargets => null;

        /// <inheritdoc />
        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.invalidater = invalidater;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Renders the specified textView information.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        public void Render(IRenderContext renderContext)
        {
            renderContext.Home();
            var line1 = $"Volume: {this.volumeService.Volume.ToString(0),5}{' '.Repeat(renderContext.Size.Width - 5 + 8)}";
            if (this.volumeService.IsMuted)
            {
                line1 = "Muted".LimitAndPadRight(renderContext.Size.Width - 5, ' ');
            }

            renderContext.WriteLine(line1);
        }

        /// <inheritdoc />
        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }

        private void OnVolumeServiceVolumeChanged(object sender, EventArgs e)
        {
            this.invalidater?.Invalidate();
        }
    }
}