namespace Aupli.CommandLine
{
    using System;
    using Aupli.CommandLine.Encoders.Ky040;
    using Pi.Timers;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome let's start");

            using (var connections = new Connections())
            {
                var amplifier = connections.Amplifier;
                var display = connections.Display;
                var menuButton = connections.MenuButton;
                var nextButton = connections.NextButton;
                var playPauseButton = connections.PlayPauseButton;
                var previousButton = connections.PreviousButton;
                var volumeInput = connections.VolumeInput;
                var rfidController = connections.RfidController;
                amplifier.SetShutdownState(false);
                amplifier.SetVolume(0);

                display.Clear();
                display.Home();
                var line1 = "                ";
                var line2 = "                ";
                display.WriteLine(line1);
                display.WriteLine(line2);

                menuButton.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Menu pressed    ";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("Menu button pressed");
                };

                nextButton.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Next pressed    ";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("Next button pressed");
                };

                playPauseButton.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Play pressed    ";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("PlayPause button pressed");
                    amplifier.SetVolume(20);
                };

                previousButton.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Previous pressed";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("Previous button pressed");
                };

                volumeInput.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Mute pressed    ";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("Mute button pressed");
                    amplifier.ToggleMute();
                };

                volumeInput.Rotating += (sender, eventArgs) =>
                {
                    switch (eventArgs.EncoderDirection)
                    {
                        case EncoderDirection.Clockwise:
                            line1 = "Volume up       ";
                            display.Clear();
                            display.WriteLine(line1);
                            display.WriteLine(line2);
                            Console.WriteLine("Volume up");
                            amplifier.SetVolume((byte)(amplifier.GetVolume() + 5));
                            break;
                        case EncoderDirection.CounterClockwise:
                            line1 = "Volume down     ";
                            display.Clear();
                            display.WriteLine(line1);
                            display.WriteLine(line2);
                            Console.WriteLine("Volume down");
                            amplifier.SetVolume((byte)(amplifier.GetVolume() - 5));
                            break;
                    }
                };

                rfidController.TagDetected += (sender, eventArgs) =>
                {
                    line2 = "Found:  " + eventArgs.Uid;
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine(line2);
                };

                rfidController.StartScanning();

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Timer.Sleep(TimeSpan.FromMilliseconds(100));
                    }
                }
                while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }
    }
}
