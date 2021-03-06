﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManagerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.TextView.ApplicationFramework.Input
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::Serilog;
    using global::Sundew.Base;
    using global::Sundew.Base.Text;
    using global::Sundew.TextView.ApplicationFramework.Input;
    using Sundew.Base.Collections;

    /// <summary>
    /// Logger for <see cref="InputManager" />.
    /// </summary>
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
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Logs the started frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        public void StartedInputContext(IReadOnlyList<object?> inputTargets)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{InputTargets}}", GetInputTargetsText(inputTargets));
        }

        /// <summary>
        /// Logs the added target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void AddedTarget(object inputTarget)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{InputTarget}}", inputTarget.GetType());
        }

        /// <summary>
        /// Logs the removed target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void RemovedTarget(object inputTarget)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{InputTarget}}", inputTarget.GetType());
        }

        /// <summary>
        /// Logs the ended frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        public void EndedInputContext(IReadOnlyList<object?> inputTargets)
        {
            this.log.Debug(
                $"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{InputTargets}}", GetInputTargetsText(inputTargets));
        }

        /// <summary>
        /// Logs the raising event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisingEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{EventType}} {{EventArgs}}", typeof(TEventArgs), eventArgs);
        }

        /// <summary>
        /// Logs the raised event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        public void RaisedEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{EventType}} {{EventArgs}}", typeof(TEventArgs), eventArgs);
        }

        /// <summary>
        /// Logs the raised event for input targets.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="inputTargets">The input targets.</param>
        /// <param name="eventArgs">The event arguments.</param>
        public void RaisedEventForInputTargets<TEventArgs>(InputEvent<TEventArgs> inputEvent, List<object?> inputTargets, TEventArgs eventArgs)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()}: {{InputTargets}} {{EventType}} {{EventArgs}}", GetInputTargetsText(inputTargets), typeof(TEventArgs), eventArgs);
        }

        private static string GetInputTargetsText(IReadOnlyList<object?> inputTargets)
        {
            return inputTargets.AggregateToStringBuilder(
                (builder, item) =>
                {
                    if (item != null)
                    {
                        builder.Append(item.GetType());
                    }
                    else
                    {
                        builder.Append("<null>");
                    }

                    builder.Append(',').Append(' ');
                }).ToString();
        }
    }
}