// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileLogWriter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementation of <see cref="ILog"/> that logs to a file.
    /// </summary>
    /// <seealso cref="ILog" />
    public class FileLogWriter : ILogWriter
    {
        private const string LogFilePathFormat = "{0}_{1}{2}";
        private const string DateTimeAsFileNameFormat = "yyyy-MM-dd_hh.mm.ss.fffffff-HH.mm";
        private const long DefaultMaxLogFileSizeInBytes = 5_000_000;
        private const int DefaultMaxNumberOfLogFiles = 10;
        private readonly string logPath;
        private readonly long maxLogFileSizeInBytes;
        private readonly int maxNumberOfLogFiles;
        private readonly string directory;
        private readonly string baseFileName;
        private readonly string extension;
        private FileStream fileStream;
        private StreamWriter streamWriter;
        private int numberOfLogFiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogWriter" /> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <param name="maxLogFileSizeInBytes">Maximum size of the log file.</param>
        /// <param name="maxNumberOfLogFiles">The maximum number of log files.</param>
        public FileLogWriter(string logPath, long maxLogFileSizeInBytes = DefaultMaxLogFileSizeInBytes, int maxNumberOfLogFiles = DefaultMaxNumberOfLogFiles)
        {
            this.logPath = logPath;
            this.maxLogFileSizeInBytes = maxLogFileSizeInBytes;
            this.maxNumberOfLogFiles = maxNumberOfLogFiles;
            this.directory = Path.GetDirectoryName(this.logPath);
            this.baseFileName = Path.GetFileNameWithoutExtension(this.logPath);
            this.extension = Path.HasExtension(this.logPath) ? Path.GetExtension(this.logPath) : ".log";
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Directory.CreateDirectory(this.directory);
            this.numberOfLogFiles = Directory.EnumerateFiles(this.directory, "*" + this.extension).Count();
            this.CreateNewLogFile();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>An async task.</returns>
        public async Task WriteAsync(string message)
        {
            await this.streamWriter.WriteLineAsync(message);
            if (this.fileStream.Length >= this.maxLogFileSizeInBytes)
            {
                await this.streamWriter?.FlushAsync();
                this.DisposeCurrentFileStream();
                this.CreateNewLogFile();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.DisposeCurrentFileStream();
        }

        private static string GetDateTimeFileName(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeAsFileNameFormat, CultureInfo.InvariantCulture);
        }

        private void DisposeCurrentFileStream()
        {
            this.streamWriter?.Dispose();
            this.fileStream?.Dispose();
            this.streamWriter = null;
            this.fileStream = null;
        }

        private void CreateNewLogFile()
        {
            if (this.numberOfLogFiles >= this.maxNumberOfLogFiles)
            {
                var logFiles = Directory.EnumerateFiles(
                        this.directory,
                        string.Format(LogFilePathFormat, this.baseFileName, "*", this.extension))
                    .OrderBy(x => new FileInfo(x).CreationTime);
                var logFileToBeDeleted = logFiles.FirstOrDefault();
                if (logFileToBeDeleted != null)
                {
                    File.Delete(logFileToBeDeleted);
                }
            }
            else
            {
                this.numberOfLogFiles++;
            }

            var logFileName = Path.Combine(this.directory, string.Format(LogFilePathFormat, this.baseFileName, GetDateTimeFileName(DateTime.Now), this.extension));
            this.fileStream = new FileStream(logFileName, FileMode.Create);
            this.streamWriter = new StreamWriter(this.fileStream);
        }
    }
}