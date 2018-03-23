// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.CommandLine;
    using Pi.IO.GeneralPurpose;
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
            {
                var logger = log.ForContext<Program>();
                try
                {
                    logger.Information("------------------------------------------");
                    logger.Information("Starting Aupli: {Args}", string.Join(" ", args));
                    var configurationFactory = new ConfigurationFactory();
                    var settings = await configurationFactory.GetSettingsAsync();
                    using (var cancellationTokenSource = new CancellationTokenSource())
                    using (var gpioConnectionDriver = new GpioConnectionDriverFactory().Create())
                    using (var connectionFactory = new ConnectionFactory(gpioConnectionDriver, settings.Pin26Feature, settings.Volume, log))
                    using (var viewRendererFactory = new ViewRendererFactory(connectionFactory, log))
                    using (var controllerFactory = new ControllerFactory(
                        connectionFactory,
                        configurationFactory,
                        viewRendererFactory,
                        result.Value.AllowShutdown,
                        cancellationTokenSource,
                        log))
                    {
                        try
                        {
                            var aupliController = await controllerFactory.GetAupliControllerAsync();
                            await aupliController.StartAsync();
                            logger.Information("Started Aupli");
                            while (!cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationTokenSource.Token);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                        }

                        await configurationFactory.SaveSettingsAsync();
                        await configurationFactory.SavePlaylistMapAsync();
                    }

                    logger.Information("Stopped Aupli");
                }
                catch (Exception e)
                {
                    logger.Error(e, "Aupli crashed");
                    return 1;
                }
            }

            return 0;
        }

        private static ILogger GetLog(Result<Options, ParserError<int>> result)
        {
            var options = result.Value;
            var logConfiguration = new LoggerConfiguration().MinimumLevel.Is(options.LogLevel);
            if (options.IsLoggingToConsole)
            {
                logConfiguration = logConfiguration.WriteTo.Console();
            }

            if (options.FileLogOptions != null)
            {
                var fileLoggingOptions = options.FileLogOptions;
                logConfiguration = logConfiguration.WriteTo.File(
                    fileLoggingOptions.LogPath,
                    fileSizeLimitBytes: fileLoggingOptions.MaxLogFileSizeInBytes,
                    retainedFileCountLimit: fileLoggingOptions.MaxNumberOfLogFiles);
            }

            return logConfiguration.CreateLogger();
        }
    }
}
