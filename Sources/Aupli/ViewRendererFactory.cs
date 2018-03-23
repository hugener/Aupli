// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewRendererFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using Aupli.Logging.Pi.ApplicationFramework.ViewRendering;
    using Pi.Timers;
    using Serilog;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;
    using Sundew.Pi.IO.Drivers.Displays.Hd44780;

    /// <inheritdoc />
    /// <summary>
    /// Factory for creating a view renderer.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class ViewRendererFactory : IDisposable
    {
        private readonly Lazy<TextViewRenderer> viewRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewRendererFactory" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="logger">The logger.</param>
        public ViewRendererFactory(ConnectionFactory connectionFactory, ILogger logger)
        {
            this.viewRenderer = new Lazy<TextViewRenderer>(() =>
            {
                var renderContextFactory = new RenderingContextFactory(connectionFactory.Lcd.Device, connectionFactory.Lcd.Settings);
                return new TextViewRenderer(renderContextFactory, new TimerFactory(), new TextViewRendererLogger(logger));
            });
        }

        /// <summary>
        /// Gets the textView renderer.
        /// </summary>
        /// <value>
        /// The textView renderer.
        /// </value>
        public TextViewRenderer TextViewRenderer => this.viewRenderer.Value;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.viewRenderer.IsValueCreated)
            {
                this.viewRenderer.Value.Dispose();
            }
        }
    }
}