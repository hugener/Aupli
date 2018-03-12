// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextScroller.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering.Animation
{
    using System;
    using Sundew.Base.Text;

    /// <summary>
    /// Scrolls text based on a timer.
    /// </summary>
    public class TextScroller : ITextAnimator
    {
        private readonly IInvalidater invalidater;
        private readonly ScrollMode scrollMode;
        private readonly TimeSpan startDelay;
        private readonly TimeSpan pauseDelay;
        private readonly IViewTimer timer;
        private string text;
        private int ticks;
        private Direction direction;
        private int width;
        private bool didAnimate;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextScroller" /> class.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="scrollMode">The scroll mode.</param>
        /// <param name="startDelay">The start delay.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="pauseDelay">The pause delay.</param>
        public TextScroller(IInvalidater invalidater, ScrollMode scrollMode, TimeSpan startDelay, TimeSpan interval, TimeSpan pauseDelay)
        {
            this.invalidater = invalidater;
            this.scrollMode = scrollMode;
            this.startDelay = startDelay;
            this.pauseDelay = pauseDelay;
            this.timer = invalidater.CreateTimer();
            this.timer.Tick += this.OnTimerTick;
            this.timer.Interval = interval;
        }

        private enum Direction
        {
            Left,
            Right,
        }

        /// <summary>
        /// Gets a value indicating whether this instance is changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsChanged => !this.didAnimate;

        /// <summary>
        /// Scrolls the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>
        /// A frame of the animated text.
        /// </returns>
        public string GetFrame(string text, int width, int height)
        {
            if (this.text != text || this.width != width)
            {
                this.text = text;
                this.width = width;
                this.direction = Direction.Right;
                this.ticks = 0;
                this.timer.Restart(this.startDelay);
            }

            this.didAnimate = true;
            return this.text.Substring(this.ticks).LimitAndPadRight(this.width, ' ');
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            if (!this.didAnimate)
            {
                this.timer.Stop();
                this.text = null;
                this.width = 0;
                return;
            }

            switch (this.scrollMode)
            {
                case ScrollMode.Bounce:
                case ScrollMode.Restart:
                    this.AnimateBounceAndRestart();
                    break;
            }

            this.didAnimate = false;
            this.invalidater.Invalidate();
        }

        private void AnimateBounceAndRestart()
        {
            if (this.text.Length > this.width)
            {
                switch (this.direction)
                {
                    case Direction.Left:
                        if (this.ticks > 0)
                        {
                            this.ticks--;
                        }
                        else if (this.ticks == 0)
                        {
                            this.direction = Direction.Right;
                            this.timer.Restart(this.startDelay);
                        }

                        break;
                    default:
                        var actualTextLength = this.text.Length - this.ticks;
                        if (actualTextLength > this.width)
                        {
                            this.ticks++;
                        }
                        else if (actualTextLength == this.width)
                        {
                            if (this.scrollMode == ScrollMode.Restart)
                            {
                                this.direction = Direction.Right;
                                this.ticks = 0;
                            }
                            else
                            {
                                this.direction = Direction.Left;
                            }

                            this.timer.Restart(this.pauseDelay);
                        }

                        break;
                }
            }
        }
    }
}