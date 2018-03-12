// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewTimer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections.Generic;
    using global::Pi.Timers;

    internal class ViewTimer : IViewTimer, IDisposable
    {
        private readonly ITimer timer;
        private readonly LinkedList<EventHandler> tick = new LinkedList<EventHandler>();
        private bool isListening = false;

        public ViewTimer(ITimer timer)
        {
            this.timer = timer;
        }

        public event EventHandler Tick
        {
            add => this.tick.AddLast(value);
            remove => this.tick.Remove(value);
        }

        public TimeSpan Interval
        {
            get => this.timer.Interval;
            set => this.timer.Interval = value;
        }

        public void Start(TimeSpan startDelay)
        {
            this.AttachToTimerTick();
            this.timer.Start(startDelay);
        }

        public void Stop()
        {
            this.timer.Stop();
            this.DetachFromTimerTick();
        }

        public void Restart(TimeSpan startDelay)
        {
            if (!this.isListening)
            {
                this.AttachToTimerTick();
            }

            this.timer.Restart(startDelay);
        }

        public void Reset()
        {
            this.Stop();
            this.tick.Clear();
        }

        public void Dispose()
        {
            this.Reset();
            this.timer.Dispose();
        }

        private void OnTimerTick(object sender, EventArgs eventArgs)
        {
            foreach (var eventHandler in this.tick)
            {
                eventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AttachToTimerTick()
        {
            this.isListening = true;
            this.timer.Tick += this.OnTimerTick;
        }

        private void DetachFromTimerTick()
        {
            this.timer.Tick -= this.OnTimerTick;
            this.isListening = false;
        }
    }
}