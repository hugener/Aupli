// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManager.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Input
{
    using System.Collections.Generic;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Manages which listener are notified by <see cref="InputEvent{TEventArgs}"/>.
    /// </summary>
    public sealed class InputManager
    {
        private readonly Stack<List<object>> inputTargetStack = new Stack<List<object>>();
        private readonly IInputManagerObserver inputManagerObserver;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="inputManagerObserver">The input manager logger.</param>
        public InputManager(ILog log, IInputManagerObserver inputManagerObserver = null)
        {
            this.logger = log.GetCategorizedLogger(typeof(InputManager), true);
            this.inputManagerObserver = inputManagerObserver;
        }

        /// <summary>
        /// Starts the frame.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void StartFrame(object inputTarget)
        {
            this.logger.LogDebug("StartFrame: " + inputTarget.GetType().Name);
            this.inputTargetStack.Push(new List<object> { inputTarget });
            this.inputManagerObserver?.StartedFrame(inputTarget);
        }

        /// <summary>
        /// Adds the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void AddTarget(object inputTarget)
        {
            this.logger.LogDebug("AddTarget: " + inputTarget.GetType().Name);
            this.inputTargetStack.Peek().Add(inputTarget);
            this.inputManagerObserver?.AddedTarget(inputTarget);
        }

        /// <summary>
        /// Removes the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void RemoveTarget(object inputTarget)
        {
            this.logger.LogDebug("RemoveTarget: " + inputTarget.GetType().Name);
            this.inputTargetStack.Peek().Remove(inputTarget);
            this.inputManagerObserver?.RemovedTarget(inputTarget);
        }

        /// <summary>
        /// Ends the frame.
        /// </summary>
        public void EndFrame()
        {
            var inputTarget = this.inputTargetStack.Pop();
            this.logger.LogDebug("EndFrame: " + inputTarget.GetType().Name);
            this.inputManagerObserver?.EndedFrame(inputTarget);
        }

        /// <summary>
        /// Creates the event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <returns>An <see cref="InputEvent{TEventArgs}"/></returns>
        public InputEvent<TEventArgs> CreateEvent<TEventArgs>()
        {
            return new InputEvent<TEventArgs>();
        }

        /// <summary>
        /// Raises the specified input event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event args instance containing the event data.</param>
        public void Raise<TEventArgs>(InputEvent<TEventArgs> inputEvent, object sender, TEventArgs eventArgs)
        {
            this.inputManagerObserver?.RaisingEvent(inputEvent, eventArgs);
            inputEvent.RaiseGlobal(sender, eventArgs);
            if (this.inputTargetStack.TryPeek(out var inputTargets))
            {
                foreach (var inputTarget in inputTargets)
                {
                    inputEvent.RaiseLocal(inputTarget, sender, eventArgs);
                }
            }

            this.inputManagerObserver?.RaisedEvent(inputEvent, eventArgs);
        }
    }
}