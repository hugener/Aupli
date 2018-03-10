// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using Aupli.Numeric;
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for displaying the current volume.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class VolumeTextView : ITextView
    {
        private Percentage volume;
        private bool isMuted;
        private IInvalidater invalidater;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeTextView"/> class.
        /// </summary>
        /// <param name="volumeController">The volume controller.</param>
        public VolumeTextView(VolumeController volumeController)
        {
            volumeController.VolumeChanged += this.OnVolumeControllerVolumeChanged;
            this.volume = volumeController.Volume;
        }

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
            var line1 = $"Volume: {this.volume.ToString(0),5}{' '.Repeat(renderContext.Width - 5 + 8)}";
            if (this.isMuted)
            {
                line1 = "Muted".LimitAndPadRight(renderContext.Width - 5, ' ');
            }

            renderContext.WriteLine(line1);
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }

        private void OnVolumeControllerVolumeChanged(object sender, VolumeEventArgs e)
        {
            if (this.isMuted != e.IsMuted || this.volume != e.Volume)
            {
                this.volume = e.Volume;
                this.isMuted = e.IsMuted;
                this.invalidater.Invalidate();
            }
        }
    }
}