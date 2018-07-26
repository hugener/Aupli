// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewNavigatorLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface
{
    using Aupli.SystemBoundaries.UserInterface.RequiredInterface;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IViewNavigatorReporter"/>.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.UserInterface.RequiredInterface.IViewNavigatorReporter" />
    public class ViewNavigatorLogger : IViewNavigatorReporter
    {
        private ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNavigatorLogger"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ViewNavigatorLogger(ILogger logger)
        {
            this.log = logger.ForContext<ViewNavigatorLogger>();
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
        /// Navigates to player text view.
        /// </summary>
        public void NavigateToPlayerTextView()
        {
            this.log.Debug(nameof(this.NavigateToPlayerTextView));
        }

        /// <summary>
        /// Navigates to volume text view.
        /// </summary>
        public void NavigateToVolumeTextView()
        {
            this.log.Debug(nameof(this.NavigateToVolumeTextView));
        }

        /// <summary>
        /// Navigates to menu text view.
        /// </summary>
        public void NavigateToMenuTextView()
        {
            this.log.Debug(nameof(this.NavigateToMenuTextView));
        }

        /// <summary>
        /// Navigates to shutdown text view.
        /// </summary>
        public void NavigateToShutdownTextView()
        {
            this.log.Debug(nameof(this.NavigateToShutdownTextView));
        }

        /// <summary>
        /// Navigates the back.
        /// </summary>
        public void NavigateBack()
        {
            this.log.Debug(nameof(this.NavigateBack));
        }
    }
}