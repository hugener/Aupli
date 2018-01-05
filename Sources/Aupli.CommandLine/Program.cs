using MpcNET.Commands;

namespace Aupli.CommandLine
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Aupli.CommandLine.Encoders.Ky040;
    using MpcNET;
    using MpcNET.Tags;
    using Pi.Timers;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome let's start");

            var mpcConnection = new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600));
            await mpcConnection.ConnectAsync();
            await mpcConnection.SendAsync(Command.Database.Update());
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
                    amplifier.SetVolume((byte)(amplifier.GetVolume() + 5));
                };

                playPauseButton.Pressed += async (sender, eventArgs) =>
                {
                    line1 = "Play pressed    ";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("PlayPause button pressed");
                    await mpcConnection.SendAsync(Command.Playback.PlayPause());
                };

                previousButton.Pressed += (sender, eventArgs) =>
                {
                    line1 = "Previous pressed";
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine("Previous button pressed");
                    amplifier.SetVolume((byte)(amplifier.GetVolume() - 5));
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

                rfidController.TagDetected += async (sender, eventArgs) =>
                {
                    line2 = "Found:  " + eventArgs.Uid;
                    display.Clear();
                    display.WriteLine(line1);
                    display.WriteLine(line2);
                    Console.WriteLine(line2);
                    var tagDirectory = eventArgs.Uid.ToString();
                    var findMpcMessage = await mpcConnection.SendAsync(Command.Database.Find(FindTags.Base, tagDirectory));
                    if (findMpcMessage.IsResponseValid)
                    {
                        var statusMessage = await mpcConnection.SendAsync(Command.Status.GetStatus());
                        if (statusMessage.IsResponseValid)
                        {
                            if (findMpcMessage.Response.Body.FirstOrDefault(x =>
                                    x.Id == statusMessage.Response.Body.SongId) == null)
                            {
                                Console.WriteLine("Clear playlist");
                                await mpcConnection.SendAsync(Command.Playlists.Current.Clear());
                                Console.WriteLine("Add songs to playlist");
                                await mpcConnection.SendAsync(Command.Playlists.Current.AddDirectory(tagDirectory));
                                var firstSong = findMpcMessage.Response.Body.First();
                                Console.WriteLine($"Start song {firstSong.Title} playlist");
                                await mpcConnection.SendAsync(Command.Playback.Play(firstSong));
                            }
                            else
                            {
                                Console.WriteLine("Playlist already playing");
                            }
                        }
                    }
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

                if (mpcConnection.IsConnected)
                {
                    await mpcConnection.DisconnectAsync();
                }
            }
        }
    }
}
