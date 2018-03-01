// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManagerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Pi.ApplicationFramework.Input
{
    using System.Collections.Generic;
    using System.Linq;
    using Sundew.Pi.ApplicationFramework.Input;
    using Sundew.Pi.ApplicationFramework.Logging;

    /// <summary>
    /// Logger for <see cref="InputManager"/>.
    /// </summary>
    /// <seealso cref="IInputManagerObserver" />
    public class InputManagerLogger : IInputManagerObserver
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManagerLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public InputManagerLogger(ILog log)
        {
            this.logger = log.GetCategorizedLogger<InputManagerLogger>(true);
        }

        /// <summary>
        /// Logs the started frame.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void StartedFrame(object inputTarget)
        {
            this.logger.LogDebug($"{nameof(this.StartedFrame)}: {inputTarget.GetType()}");
        }

        /// <summary>
        /// Logs the added target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void AddedTarget(object inputTarget)
        {
            this.logger.LogDebug($"{nameof(this.AddedTarget)}: {inputTarget.GetType()}");
        }

        /// <summary>
        /// Logs the removed target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void RemovedTarget(object inputTarget)
        {
            this.logger.LogDebug($"{nameof(this.RemovedTarget)}: {inputTarget.GetType()}");
        }

        /// <summary>
        /// Logs the ended frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        public void EndedFrame(IReadOnlyList<object> inputTargets)
        {
            this.logger.LogDebug($"{nameof(this.EndedFrame)}: {string.Join(", ", inputTargets.Select(x => x.GetType()))}");
        }

        /// <summary>
        /// Logs the raising event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisingEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.logger.LogDebug($"{nameof(this.RaisingEvent)}: {typeof(TEventArgs)} {eventArgs}");
        }

        /// <summary>
        /// Logs the raised event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisedEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.logger.LogDebug($"{nameof(this.RaisedEvent)}: {typeof(TEventArgs)} {eventArgs}");
        }
    }
}