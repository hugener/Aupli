// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.CommandLine;
    using Aupli.OperationSystem;
    using Newtonsoft.Json;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Computation;
    using Sundew.CommandLine;
    using Sundew.Pi.ApplicationFramework.Logging;

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
                new Options(false, true, LogLevel.Debug, new FileLogOptions()),
                options => Result.Success(options));
            var result = commandLineParser.Parse(args);
            if (!result)
            {
                Console.WriteLine("Aupli: Invalid command line:");
                Console.WriteLine(commandLineParser.CreateHelpText());
                return 1;
            }

            using (var log = GetLog(result))
            {
                var logger = log.GetCategorizedLogger(typeof(Program).Name);
                try
                {
                    logger.LogInfo("------------------------------------------");
                    logger.LogInfo("Starting Aupli: " + string.Join(" ", args));
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
                            logger.LogInfo("Started Aupli");
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

                        logger.LogInfo("Stopped Aupli");
                        if (result.Value.AllowShutdown)
                        {
                            logger.LogInfo("Shutting down Aupli");
                            await Task.Delay(TimeSpan.FromSeconds(3.5), CancellationToken.None);
                            var powerConnection = new PowerConnection();
                            powerConnection.Shutdown();
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e.ToString());
                    logger.LogError("Aupli crashed");
                    return 1;
                }
            }

            return 0;
        }

        private static Log GetLog(Result<Options, ParserError<int>> result)
        {
            var options = result.Value;
            var logWriters = new List<ILogWriter>();
            if (options.IsLoggingToConsole)
            {
                logWriters.Add(new ConsoleLogWriter());
            }

            if (options.FileLogOptions != null)
            {
                var fileLoggingOptions = options.FileLogOptions;
                logWriters.Add(new FileLogWriter(
                    fileLoggingOptions.LogPath,
                    fileLoggingOptions.MaxLogFileSizeInBytes,
                    fileLoggingOptions.MaxNumberOfLogFiles));
            }

            return new Log(new MultiLogWriter(logWriters), options.LogLevel);
        }
    }
}
