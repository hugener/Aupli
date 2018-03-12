// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlinker.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering.Animation
{
    using System;
    using Sundew.Base.Text;

    /// <summary>
    /// Text animation that blinks the text.
    /// </summary>
    public class TextBlinker : ITextAnimator
    {
        private readonly IInvalidater invalidater;
        private readonly IViewTimer viewTimer;
        private bool didAnimate;
        private string text;
        private bool isOn = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlinker"/> class.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="interval">The interval.</param>
        public TextBlinker(IInvalidater invalidater, TimeSpan interval)
        {
            this.invalidater = invalidater;
            this.viewTimer = invalidater.CreateTimer();
            this.viewTimer.Tick += this.OnViewTimerTick;
            this.viewTimer.Interval = interval;
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>
        /// The text to be displayed.
        /// </returns>
        public string GetFrame(string text, int width, int height)
        {
            if (this.text != text)
            {
                this.text = text;
                this.viewTimer.Restart(this.viewTimer.Interval);
            }

            this.didAnimate = true;
            return this.isOn ? this.text : ' '.Repeat(this.text.Length);
        }

        private void OnViewTimerTick(object sender, EventArgs e)
        {
            if (!this.didAnimate)
            {
                this.viewTimer.Stop();
                this.text = null;
                return;
            }

            this.isOn = !this.isOn;
            this.didAnimate = false;
            this.invalidater.Invalidate();
        }
    }
}