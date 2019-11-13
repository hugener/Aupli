// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRendererLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.TextView.ApplicationFramework.ViewRendering
{
    using System;
    using System.Reflection;
    using global::Serilog;
    using global::Sundew.Base;
    using global::Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Logger for the <see cref="TextViewRenderer"/>.
    /// </summary>
    /// <seealso cref="ITextViewRendererReporter" />
    public class TextViewRendererLogger : ITextViewRendererReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRendererLogger" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TextViewRendererLogger(ILogger logger)
        {
            this.log = logger.ForContext<TextViewRendererLogger>();
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
                GetViewName(newTextView),
                GetViewName(oldTextView));
        }

        /// <summary>Called when rendering.</summary>
        /// <param name="currentTextView">The current text view.</param>
        public void OnDraw(ITextView currentTextView)
        {
            this.log.Verbose("Drawing view: {TextView}", GetViewName(currentTextView));
        }

        /// <summary>Called when rendered.</summary>
        /// <param name="currentTextView">The current text view.</param>
        /// <param name="renderingContext">The rendering context.</param>
        public void OnRendered(ITextView currentTextView, IRenderingContext renderingContext)
        {
            this.log.Verbose("Rendered view: {TextView} with number of instructions: {Instructions}", GetViewName(currentTextView), renderingContext.InstructionCount);
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
            this.log.Error(exception, nameof(this.OnRendererException));
        }

        /// <summary>Waiting for access to change view.</summary>
        /// <param name="view">The view.</param>
        public void WaitingForAccessToChangeViewTo(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        /// <summary>Waiting for rendering to abort.</summary>
        /// <param name="view">The view.</param>
        public void WaitingForRenderingAborted(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        /// <summary>Waiting for access to render view.</summary>
        public void WaitingForAccessToRenderView()
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>Acquired the view for rendering.</summary>
        /// <param name="view">The view.</param>
        public void AcquiredViewForRendering(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        /// <summary>Aborting the rendering.</summary>
        /// <param name="view">The view.</param>
        public void AbortingRendering(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        /// <summary>Waiting for view to invalidate.</summary>
        /// <param name="view">The view.</param>
        public void WaitingForViewToInvalidate(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        /// <summary>Views the already set.</summary>
        /// <param name="view">The view.</param>
        public void ViewAlreadySet(ITextView view)
        {
            this.log.Verbose(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase() + ": {view}", GetViewName(view));
        }

        private static string GetViewName(ITextView view)
        {
            return view?.GetType().Name ?? "<None>";
        }
    }
}