// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartUpController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Lifecycle
{
    using System;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Handles the startup process.
    /// </summary>
    public class StartUpController
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly TextViewRenderer textViewRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartUpController"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="textViewRenderer">The textView renderer.</param>
        public StartUpController(ConnectionFactory connectionFactory, TextViewRenderer textViewRenderer)
        {
            this.connectionFactory = connectionFactory;
            this.textViewRenderer = textViewRenderer;
        }

        /// <summary>
        /// Occurs when textView rendering has benn initialized.
        /// </summary>
        public event EventHandler ViewRenderingInitialized;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            this.textViewRenderer.Start();
            this.ViewRenderingInitialized?.Invoke(this, EventArgs.Empty);
            this.connectionFactory.Initialize();
        }
    }
}