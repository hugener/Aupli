// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRendererLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Pi.ApplicationFramework.ViewRendering
{
    using System;
    using Serilog;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Logger for the <see cref="TextViewRenderer"/>.
    /// </summary>
    /// <seealso cref="ITextViewRendererObserver" />
    public class TextViewRendererLogger : ITextViewRendererObserver
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRendererLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TextViewRendererLogger(ILogger logger)
        {
            this.log = logger.ForContext<TextViewRendererLogger>();
        }

        /// <summary>
        /// Logs starteds.
        /// </summary>
        public void Started()
        {
            this.log.Information(nameof(this.Started));
        }

        /// <summary>
        /// Logs the changed views.
        /// </summary>
        /// <param name="newTextView">The new text view.</param>
        /// <param name="oldTextView">The old text view.</param>
        public void OnViewChanged(ITextView newTextView, ITextView oldTextView)
        {
            this.log.Information(
                $"{nameof(this.OnViewChanged)} to {{NewView}} from {{OldView}}",
                newTextView.GetType().Name,
                oldTextView != null ? oldTextView.GetType().Name : "<None>");
        }

        /// <summary>
        /// Logs stopped.
        /// </summary>
        public void Stopped()
        {
            this.log.Information(nameof(this.Stopped));
        }

        /// <summary>
        /// Called when [renderer exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void OnRendererException(Exception exception)
        {
            this.log.Information(exception, nameof(this.OnRendererException));
        }
    }
}