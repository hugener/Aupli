// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Text;

    /// <summary>
    /// Represents a log.
    /// </summary>
    /// <seealso cref="ILog" />
    public class Log : ILog
    {
        private readonly ILogWriter logWriter;
        private readonly BlockingCollection<string> logBlockingCollection = new BlockingCollection<string>(new ConcurrentQueue<string>());
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Task logWriterTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="logLevel">The log level.</param>
        public Log(ILogWriter logWriter, LogLevel logLevel)
        {
            this.LogLevel = logLevel;
            this.logWriter = logWriter;
            this.logWriterTask = new TaskFactory().StartNew(
                this.OutputLog,
                this.cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the categorized logger.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>A new <see cref="CategorizedLogger"/>.</returns>
        public ILogger GetCategorizedLogger(string category)
        {
            return new CategorizedLogger(this, category);
        }

        /// <summary>
        /// Logs the trace.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="category">The category.</param>
        /// <param name="dateTime">The date time.</param>
        /// <param name="message">The message.</param>
        public void LogMessage(LogLevel logLevel, string category, DateTime dateTime, string message)
        {
            if (this.LogLevel <= logLevel && !this.cancellationTokenSource.IsCancellationRequested)
            {
                this.logBlockingCollection.Add(GetMessage(logLevel, category, dateTime, message));
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
            this.logWriterTask.Wait();
            this.logBlockingCollection.Dispose();
            this.cancellationTokenSource.Dispose();
            this.logWriterTask.Dispose();
            this.logWriter?.Dispose();
        }

        private static string GetMessage(LogLevel logLevel, string category, DateTime dateTime, string message)
        {
            return
                $"{dateTime:G} | {logLevel.ToString().LimitAndPadRight(7, ' ')} | {category.LimitAndPadRight(25, ' ')} | {message}";
        }

        private async void OutputLog()
        {
            try
            {
                this.logWriter.Initialize();
                var cancellationToken = this.cancellationTokenSource.Token;
                while (!cancellationToken.IsCancellationRequested)
                {
                    var logItem = this.logBlockingCollection.Take(cancellationToken);
                    await this.logWriter.WriteAsync(logItem);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}