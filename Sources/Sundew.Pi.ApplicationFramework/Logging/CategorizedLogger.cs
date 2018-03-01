// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizedLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    internal class CategorizedLogger : ILogger
    {
        private readonly ILog log;
        private readonly string category;

        public CategorizedLogger(ILog log, string category)
        {
            this.log = log;
            this.category = category;
        }

        public void Log(LogLevel logLevel, DateTime dateTime, string message)
        {
            this.log.LogMessage(logLevel, this.category, dateTime, message);
        }
    }
}