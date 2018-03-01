// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdleController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Input
{
    using System;
    using System.Threading;
    using global::Pi.Timers;

    /// <summary>
    /// Tracks whether the application receives input or has activity.
    /// </summary>
    public class IdleController : IDisposable
    {
        private readonly IInputAggregator inputAggregator;
        private readonly IActivityAggregator activityAggregator;
        private readonly TimeSpan inputIdleTimeSpan;
        private readonly TimeSpan systemIdleTimeSpan;
        private readonly IIdleControllerObserver idleControllerObserver;
        private readonly object lockObject = new object();
        private readonly ITimer inputIdleTimer;
        private readonly ITimer systemIdleTimer;
        private bool isInputIdle;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleController"/> class.
        /// </summary>
        /// <param name="inputAggregator">The input aggregator.</param>
        /// <param name="activityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The systemidle time span.</param>
        /// <param name="idleControllerObserver">The idle controller observer.</param>
        public IdleController(
            IInputAggregator inputAggregator,
            IActivityAggregator activityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan,
            IIdleControllerObserver idleControllerObserver)
        {
            this.inputAggregator = inputAggregator;
            this.activityAggregator = activityAggregator;
            this.inputIdleTimeSpan = inputIdleTimeSpan;
            this.systemIdleTimeSpan = systemIdleTimeSpan;
            this.idleControllerObserver = idleControllerObserver;
            this.inputAggregator.ActivityOccured += this.OnInputActivity;
            this.activityAggregator.ActivityOccured += this.OnSystemActivity;
            this.inputIdleTimer = global::Pi.Timers.Timer.Create();
            this.inputIdleTimer.Tick += this.OnInputIdle;
            this.inputIdleTimer.Interval = Timeout.InfiniteTimeSpan;
            this.systemIdleTimer = global::Pi.Timers.Timer.Create();
            this.systemIdleTimer.Tick += this.OnSystemIdle;
            this.systemIdleTimer.Interval = Timeout.InfiniteTimeSpan;
        }

        /// <summary>
        /// Occurs when the application has not received input for a given time.
        /// </summary>
        public event EventHandler InputIdle;

        /// <summary>
        /// Occurs when the application has not had activity for a given time.
        /// </summary>
        public event EventHandler SystemIdle;

        /// <summary>
        /// Occurs when the application received input after being idle.
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Gets a value indicating whether this instance is input idle.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is input idle; otherwise, <c>false</c>.
        /// </value>
        public bool IsInputIdle => this.isInputIdle;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.inputAggregator.Start();
            lock (this.lockObject)
            {
                this.inputIdleTimer.Start(this.inputIdleTimeSpan);
                this.systemIdleTimer.Start(this.systemIdleTimeSpan);
            }

            this.Activated?.Invoke(this, EventArgs.Empty);
            this.idleControllerObserver?.Started();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            global::Pi.Timers.Timer.Dispose(this.inputIdleTimer);
            global::Pi.Timers.Timer.Dispose(this.systemIdleTimer);
        }

        private void OnInputActivity(object sender, EventArgs eventArgs)
        {
            lock (this.lockObject)
            {
                var oldIsInputIdle = this.isInputIdle;
                this.systemIdleTimer.Stop();
                this.systemIdleTimer.Start(this.systemIdleTimeSpan);
                this.isInputIdle = false;
                this.inputIdleTimer.Stop();
                this.inputIdleTimer.Start(this.inputIdleTimeSpan);
                this.idleControllerObserver?.OnInputActivity();

                if (oldIsInputIdle)
                {
                    this.Activated?.Invoke(this, EventArgs.Empty);
                    this.idleControllerObserver?.Activated();
                }
            }
        }

        private void OnSystemActivity(object sender, EventArgs eventArgs)
        {
            lock (this.lockObject)
            {
                this.systemIdleTimer.Stop();
                this.systemIdleTimer.Start(this.systemIdleTimeSpan);
                this.idleControllerObserver?.OnSystemActivity();
            }
        }

        private void OnInputIdle(object sender, EventArgs e)
        {
            lock (this.lockObject)
            {
                this.inputIdleTimer.Stop();
                var oldIsInputIdle = this.isInputIdle;
                this.isInputIdle = true;

                if (!oldIsInputIdle)
                {
                    this.idleControllerObserver?.OnInputIdle();
                    this.InputIdle?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void OnSystemIdle(object sender, EventArgs e)
        {
            lock (this.lockObject)
            {
                this.systemIdleTimer.Stop();
                this.idleControllerObserver?.OnSystemIdle();
                this.SystemIdle?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}