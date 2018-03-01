// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Drivers.Displays.Hd44780
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using global::Pi.IO.Components.Displays.Hd44780;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Render context for rendering on a <see cref="Hd44780LcdConnection"/>.
    /// </summary>
    /// <seealso cref="IRenderContext" />
    public class RenderingContext : IRenderingContext
    {
        private readonly Hd44780LcdConnection hd44780LcdConnection;
        private readonly LinkedList<Action> renderInstructions = new LinkedList<Action>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingContext" /> class.
        /// </summary>
        /// <param name="hd44780LcdConnection">The HD44780 LCD connection.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RenderingContext(Hd44780LcdConnection hd44780LcdConnection, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.hd44780LcdConnection = hd44780LcdConnection;
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            this.renderInstructions.AddLast(() => this.hd44780LcdConnection.WriteLine(text));
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            this.renderInstructions.AddLast(() => this.hd44780LcdConnection.Write(text));
        }

        /// <summary>
        /// Homes this instance.
        /// </summary>
        public void Home()
        {
            this.renderInstructions.AddLast(() => this.hd44780LcdConnection.Home());
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.renderInstructions.AddLast(() => this.hd44780LcdConnection.Clear());
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            this.renderInstructions.AddLast(() =>
                this.hd44780LcdConnection.SetCursorPosition(new Hd44780Position { Column = x, Row = y }));
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The render instructions.</returns>
        public IEnumerator<Action> GetEnumerator()
        {
            return this.renderInstructions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}