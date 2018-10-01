// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SplashScreen
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Pi.IO.Devices.Displays.Hd44780;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Text;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        private const int ScreenWidth = 16;

        /// <summary>
        /// Prints welcome to the screen.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>An async task.</returns>
        public static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var hd44780LcdDeviceSettings = new Hd44780LcdDeviceSettings
                {
                    ScreenHeight = 2,
                    ScreenWidth = ScreenWidth,
                    Encoding = new Hd44780A00Encoding(),
                };
                using (var gpioConnectionDriverFactory = new GpioConnectionDriverFactory(true))
                {
                    using (var hd44780LcdDevice = new Hd44780LcdDevice(
                        hd44780LcdDeviceSettings,
                        gpioConnectionDriverFactory,
                        ConnectorPin.P1Pin29,
                        ConnectorPin.P1Pin32,
                        new Hd44780DataPins(
                            ConnectorPin.P1Pin31,
                            ConnectorPin.P1Pin33,
                            ConnectorPin.P1Pin35,
                            ConnectorPin.P1Pin37),
                        null))
                    {
                        hd44780LcdDevice.IsClearingOnClose = false;
                        hd44780LcdDevice.Clear();
                        var greeting = await GreetingsProvider.GetGreetingAsync();
                        var name = await NameProvider.GetNameAsync();
                        hd44780LcdDevice.WriteLine($"{greeting} {name}".LimitAndPadRight(ScreenWidth, ' '));
                        Console.WriteLine($"{greeting} {name}");

                        await GreetingsProvider.SaveLastGreetingAsync(greeting);
                    }
                }

                Console.WriteLine("StartUp time: " + stopwatch.Elapsed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
