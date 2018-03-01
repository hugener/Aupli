// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRenderer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Collections;
    using Sundew.Base.Computation;

    /// <inheritdoc />
    /// <summary>
    /// Renders textView on to a Hd44780 LCD display.
    /// </summary>
    /// <seealso cref="T:IDisposable" />
    public class TextViewRenderer : IDisposable
    {
        private readonly object renderLock = new object();
        private readonly IRenderingContextFactory renderContextFactory;
        private readonly ITextViewRendererObserver textViewRendererObserver;
        private Task renderTask;
        private ITextView textView;
        private CancellationTokenSource cancellationTokenSource;
        private Invalidater invalidater;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRenderer" /> class.
        /// </summary>
        /// <param name="renderContextFactory">The render context factory.</param>
        /// <param name="textViewRendererObserver">The textView renderer logger.</param>
        public TextViewRenderer(IRenderingContextFactory renderContextFactory, ITextViewRendererObserver textViewRendererObserver)
        {
            this.renderContextFactory = renderContextFactory;
            this.textViewRendererObserver = textViewRendererObserver;
        }

        /// <summary>
        /// Starts rendering.
        /// </summary>
        public void Start()
        {
            if (this.renderTask != null)
            {
                return;
            }

            this.cancellationTokenSource = new CancellationTokenSource();
            lock (this.renderLock)
            {
                var renderingContext = this.renderContextFactory.CreateRenderingContext();
                renderingContext.Clear();
                renderingContext.ForEach(renderInstruction => renderInstruction());
            }

            this.renderTask = new TaskFactory().StartNew(
                this.Render,
                this.cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
            this.textViewRendererObserver?.Started();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.cancellationTokenSource?.Cancel();
            this.renderTask?.Wait();
            this.cancellationTokenSource?.Dispose();
            this.cancellationTokenSource = null;
            this.renderTask = null;
            this.textView = null;
            this.textViewRendererObserver?.Stopped();
        }

        /// <summary>
        /// Tries to set the textView.
        /// </summary>
        /// <param name="newTextView">The new textView.</param>
        /// <returns><c>true</c>, if the textView was set, otherwise <c>false</c>.</returns>
        public Result<ITextView> TrySetView(ITextView newTextView)
        {
            lock (this.renderLock)
            {
                if (this.textView != newTextView)
                {
                    var oldView = this.textView;
                    oldView?.OnClosing();
                    this.textView = newTextView;
                    this.invalidater = new Invalidater();
                    this.renderContextFactory.TryCreateCustomCharacterBuilder(out var characterContext);
                    this.textView.OnShowing(this.invalidater, characterContext);
                    this.textViewRendererObserver?.OnViewChanged(newTextView, oldView);
                    return Result.Success(oldView);
                }

                return Result.Error();
            }
        }

        private void Render()
        {
            var cancellationToken = this.cancellationTokenSource.Token;
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    lock (this.renderLock)
                    {
                        if (this.textView != null && this.invalidater.IsRenderRequiredAndReset())
                        {
                            var renderingContext = this.renderContextFactory.CreateRenderingContext();
                            this.textView.Render(renderingContext);
                            cancellationToken.ThrowIfCancellationRequested();

                            foreach (var renderInstruction in renderingContext)
                            {
                                renderInstruction();
                            }
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                this.textViewRendererObserver.OnRendererException(e);
                throw;
            }
        }
    }
}