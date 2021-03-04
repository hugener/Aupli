// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewNavigatorLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging.Serilog.SystemBoundaries.UserInterface
{
    using System;
    using System.Reflection;
    using Aupli.SystemBoundaries.UserInterface.Ari;
    using global::Serilog;
    using Sundew.Base;

    /// <summary>
    /// Logger for <see cref="IViewNavigatorReporter"/>.
    /// </summary>
    /// <seealso cref="IViewNavigatorReporter" />
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
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public void SetSource(Type target, object source)
        {
            this.log = this.log.ForContext(source.AsType());
        }

        /// <summary>
        /// Navigates to player text view.
        /// </summary>
        public void NavigateToPlayerTextView()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Navigates to volume text view.
        /// </summary>
        public void NavigateToVolumeTextView()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Navigates to menu text view.
        /// </summary>
        public void NavigateToMenuTextView()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Navigates to shutdown text view.
        /// </summary>
        public void NavigateToShutdownTextView()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }

        /// <summary>
        /// Navigates the back.
        /// </summary>
        public void NavigateBack()
        {
            this.log.Debug(MethodBase.GetCurrentMethod()!.Name.FromCamelCaseToSentenceCase());
        }
    }
}