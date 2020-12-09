// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InteractionControllerLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface.Input
{
    using System;
    using System.Reflection;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Input.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Serilog logger for <see cref="IInteractionControllerReporter"/>.
    /// </summary>
    /// <seealso cref="IInteractionControllerReporter" />
    public class InteractionControllerLogger : IInteractionControllerReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionControllerLogger"/> class.
        /// </summary>
        /// <param name="log">The logger.</param>
        public InteractionControllerLogger(ILogger log)
        {
            this.log = log.ForContext<InteractionControllerLogger>();
        }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Starteds this instance.
        /// </summary>
        public void Started()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Tags the input event.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public void TagInputEvent(string uid)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{{nameof(uid)}}}", uid);
        }

        /// <summary>
        /// Keys the input event.
        /// </summary>
        /// <param name="keyInput">The key input.</param>
        public void KeyInputEvent(KeyInput keyInput)
        {
            this.log.Debug($"{MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase()} {{{nameof(keyInput)}}}", keyInput);
        }
    }
}