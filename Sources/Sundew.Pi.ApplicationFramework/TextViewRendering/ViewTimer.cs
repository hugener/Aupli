// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewTimer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    using System;
    using global::Pi.Timers;

    internal class ViewTimer : IViewTimer, IDisposable
    {
        private readonly ITimer timer;

        public ViewTimer(ITimer timer)
        {
            this.timer = timer;
            this.Tick += this.OnTimerTick;
        }

        public event EventHandler Tick;

        public TimeSpan Interval
        {
            get => this.timer.Interval;
            set => this.timer.Interval = value;
        }

        public void Start(TimeSpan startDelay)
        {
            this.timer.Start(startDelay);
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public void Dispose()
        {
            this.timer.Tick -= this.OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            this.Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}