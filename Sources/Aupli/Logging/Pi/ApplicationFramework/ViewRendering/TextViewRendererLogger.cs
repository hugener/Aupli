// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRendererLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Pi.ApplicationFramework.ViewRendering
{
    using System;
    using Sundew.Pi.ApplicationFramework.Logging;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Logger for the <see cref="TextViewRenderer"/>.
    /// </summary>
    /// <seealso cref="ITextViewRendererObserver" />
    public class TextViewRendererLogger : ITextViewRendererObserver
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRendererLogger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public TextViewRendererLogger(ILog log)
        {
            this.logger = log.GetCategorizedLogger<TextViewRendererLogger>(true);
        }

        /// <summary>
        /// Logs starteds.
        /// </summary>
        public void Started()
        {
            this.logger.LogInfo(nameof(this.Started));
        }

        /// <summary>
        /// Logs the changed views.
        /// </summary>
        /// <param name="newTextView">The new text view.</param>
        /// <param name="oldTextView">The old text view.</param>
        public void OnViewChanged(ITextView newTextView, ITextView oldTextView)
        {
            this.logger.LogInfo($"{nameof(this.OnViewChanged)} to {newTextView.GetType().Name} from {(oldTextView != null ? oldTextView.GetType().Name : "<None>")}");
        }

        /// <summary>
        /// Logs stopped.
        /// </summary>
        public void Stopped()
        {
            this.logger.LogInfo(nameof(this.Stopped));
        }

        /// <summary>
        /// Called when [renderer exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void OnRendererException(Exception exception)
        {
            this.logger.LogError(exception.ToString());
        }
    }
}