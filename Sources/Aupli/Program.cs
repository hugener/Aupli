// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.Bootstrapping;
    using Aupli.CommandLine;
    using Serilog;
    using Serilog.Events;
    using Sundew.Base.Computation;
    using Sundew.CommandLine;

    /// <summary>
    /// The main entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>An exit code.</returns>
        public static async Task<int> Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var commandLineParser = new CommandLineParser<Options, int>();
            commandLineParser.WithArguments(
                new Options(false, true, LogEventLevel.Debug, new FileLogOptions()),
                options => Result.Success(options));
            var result = commandLineParser.Parse(args);
            if (!result)
            {
                Console.WriteLine("Aupli: Invalid command line:");
                Console.WriteLine(commandLineParser.CreateHelpText());
                return 1;
            }

            var log = GetLog(result);
            var logger = log.ForContext<Program>();
            try
            {
                logger.Information("------------------------------------------");
                logger.Information("Starting Aupli: {Args}", string.Join(" ", args));
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    var bootstrapper = new Bootstrapper(logger);
                    logger.Verbose("Created Bootstrapper");
                    try
                    {
                        await bootstrapper.StartAsync(cancellationTokenSource, result.Value.AllowShutdown);
                        logger.Information("Started Aupli in {time}", stopwatch.Elapsed);
                        stopwatch.Stop();
                        while (!cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationTokenSource.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }

                    await bootstrapper.StopAsync();
                }

                logger.Information("Stopped Aupli");
            }
            catch (Exception e)
            {
                logger.Error(e, "Aupli crashed");
                return 1;
            }

            return 0;
        }

        private static ILogger GetLog(Result<Options, ParserError<int>> result)
        {
            var options = result.Value;
            var logConfiguration = new LoggerConfiguration().MinimumLevel.Is(options.LogLevel);
            if (options.IsLoggingToConsole)
            {
                logConfiguration = logConfiguration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss.fffff} {Level:u3}] <{ThreadId,-5}> {SourceContext,-80:l} | {Message:lj}{NewLine}{Exception}");
            }

            if (options.FileLogOptions != null)
            {
                var fileLoggingOptions = options.FileLogOptions;
                logConfiguration = logConfiguration.WriteTo.Async(x => x.File(
                    fileLoggingOptions.LogPath,
                    outputTemplate: "[{Timestamp:HH:mm:ss.fffff} {Level:u3}] <{ThreadId,-5}> {SourceContext,-80:l} | {Message:lj}{NewLine}{Exception}",
                    fileSizeLimitBytes: fileLoggingOptions.MaxLogFileSizeInBytes,
                    retainedFileCountLimit: fileLoggingOptions.MaxNumberOfLogFiles));
            }

            return logConfiguration.Enrich.WithThreadId().CreateLogger();
        }
    }
}
