// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManagerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.Pi.ApplicationFramework.Input
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Serilog;
    using Sundew.Base;
    using Sundew.TextView.ApplicationFramework.Input;

    /// <summary>
    /// Logger for <see cref="InputManager" />.
    /// </summary>
    /// <seealso cref="Sundew.TextView.ApplicationFramework.Input.IInputManagerReporter" />
    /// <seealso cref="IInputManagerReporter" />
    public class InputManagerLogger : IInputManagerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManagerLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public InputManagerLogger(ILogger logger)
        {
            this.log = logger.ForContext<InputManagerLogger>();
        }

        /// <summary>
        /// Sets the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Logs the started frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        public void StartedInputContext(IReadOnlyList<object> inputTargets)
        {
            this.log.Debug($"{nameof(this.StartedInputContext)}: {{InputTargets}}", string.Join(", ", inputTargets.Select(x => x.GetType())));
        }

        /// <summary>
        /// Logs the added target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void AddedTarget(object inputTarget)
        {
            this.log.Debug($"{nameof(this.AddedTarget)}: {{InputTarget}}", inputTarget.GetType());
        }

        /// <summary>
        /// Logs the removed target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void RemovedTarget(object inputTarget)
        {
            this.log.Debug($"{nameof(this.RemovedTarget)}: {{InputTarget}}", inputTarget.GetType());
        }

        /// <summary>
        /// Logs the ended frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        public void EndedInputContext(IReadOnlyList<object> inputTargets)
        {
            this.log.Debug($"{nameof(this.EndedInputContext)}: {{InputTargets}}", string.Join(", ", inputTargets.Select(x => x.GetType())));
        }

        /// <summary>
        /// Logs the raising event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisingEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.log.Debug($"{nameof(this.RaisingEvent)}: {{EventType}} {{EventArgs}}", typeof(TEventArgs), eventArgs);
        }

        /// <summary>
        /// Logs the raised event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisedEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.log.Debug($"{nameof(this.RaisedEvent)}: {{EventType}} {{EventArgs}}", typeof(TEventArgs), eventArgs);
        }
    }
}