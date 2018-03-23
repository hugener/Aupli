// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System;
    using System.Linq;
    using System.Net;
    using Aupli.Input;
    using Aupli.Logging.Mpc;
    using Aupli.Mpc;
    using Aupli.Numeric;
    using Aupli.OperationSystem;
    using Aupli.Volume;
    using MpcNET;
    using Pi.IO;
    using Pi.IO.Devices.Displays.Hd44780;
    using Pi.IO.GeneralPurpose;
    using Serilog;
    using Sundew.Base.Disposal;
    using Sundew.Base.Numeric;
    using Sundew.Pi.IO.Devices.Amplifiers.Max9744;
    using Sundew.Pi.IO.Devices.Buttons;
    using Sundew.Pi.IO.Devices.Encoders.Ky040;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.PowerManagement;
    using Sundew.Pi.IO.Devices.RfidTransceivers.Mfrc522;

    /// <summary>
    /// Factory for creating connections to hardware components.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ConnectionFactory : IDisposable
    {
        private readonly Lazy<(Hd44780LcdDevice LcdDevice, Hd44780LcdDeviceSettings Settings)> lcdDevice;

        private readonly Lazy<InputControls> inputControls;

        private readonly Lazy<(MusicPlayer MusicPlayer, MpcConnection MpcConnection)> musicPlayer;

        private readonly Lazy<VolumeControls> volumeControls;
        private Disposer disposer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionFactory" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="pin26Feature">The pin26 feature.</param>
        /// <param name="initialVolume">The initial volume.</param>
        /// <param name="logger">The logger.</param>
        public ConnectionFactory(IGpioConnectionDriver gpioConnectionDriver, Pin26Feature pin26Feature, double initialVolume, ILogger logger)
        {
            this.lcdDevice = new Lazy<(Hd44780LcdDevice, Hd44780LcdDeviceSettings)>(() =>
            {
                var hd47780ConnectionSettings = new Hd44780LcdDeviceSettings
                {
                    ScreenHeight = 2,
                    ScreenWidth = 16,
                    Encoding = new Hd44780A00Encoding(),
                };
                var backlight = pin26Feature == Pin26Feature.Backlight
                    ? new ConnectorPin?(ConnectorPin.P1Pin26)
                    : null;
                var hd47780Connection = new Hd44780LcdDevice(
                    hd47780ConnectionSettings,
                    gpioConnectionDriver,
                    ConnectorPin.P1Pin29,
                    ConnectorPin.P1Pin32,
                    new Hd44780DataPins(
                        ConnectorPin.P1Pin31,
                        ConnectorPin.P1Pin33,
                        ConnectorPin.P1Pin35,
                        ConnectorPin.P1Pin37),
                    backlight);
                return (hd47780Connection, hd47780ConnectionSettings);
            });

            this.inputControls = new Lazy<InputControls>(() =>
            {
                var lircConnection = new LircDevice();
                var playPauseButton = new PullDownButtonDevice(ConnectorPin.P1Pin15);
                var nextButton = new PullDownButtonDevice(ConnectorPin.P1Pin18);
                var previousButton = new PullDownButtonDevice(ConnectorPin.P1Pin16);
                var menuButton = new PullDownButtonDevice(ConnectorPin.P1Pin13);
                var rfidTransceiver = new Mfrc522Connection("/dev/spidev0.0", ConnectorPin.P1Pin22);
                var ky040Connection = new Ky040Device(gpioConnectionDriver, ConnectorPin.P1Pin36, ConnectorPin.P1Pin38, ConnectorPin.P1Pin40);

                return new InputControls(playPauseButton, nextButton, previousButton, menuButton, rfidTransceiver, lircConnection, ky040Connection);
            });

            this.musicPlayer = new Lazy<(MusicPlayer, MpcConnection)>(() =>
                {
                    var musicPlayerLogger = new MusicPlayerLogger(logger);
                    var mpcConnection = new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600), null, musicPlayerLogger);
                    return (new MusicPlayer(mpcConnection, musicPlayerLogger), mpcConnection);
                });

            this.volumeControls = new Lazy<VolumeControls>(() =>
            {
                var max9744Device = new Max9744Device(
                    0x4b,
                    ConnectorPin.P1Pin07,
                    ConnectorPin.P1Pin11,
                    ProcessorPin.Pin02,
                    ProcessorPin.Pin03);
                max9744Device.SetShutdownState(false);
                var headphoneSwitch = pin26Feature == Pin26Feature.Headphone
                    ? new PullDownSwitchDevice(ConnectorPin.P1Pin26, gpioConnectionDriver)
                    : null;
                var volumeAdjuster =
                    new VolumeAdjuster(
                        new Range<byte>(
                            (byte)(max9744Device.VolumeRange.Min + 10),
                            (byte)(max9744Device.VolumeRange.Max - 30)),
                        new Percentage(initialVolume),
                        0.05);
                max9744Device.SetVolume(volumeAdjuster.Volume.AbsoluteValue);
                return new VolumeControls(max9744Device, headphoneSwitch, volumeAdjuster);
            });

            this.RemotePi = new RemotePiDevice(gpioConnectionDriver, ConnectorPin.P1Pin08, ConnectorPin.P1Pin10, new RasbianShutdown());
        }

        /// <summary>
        /// Gets the LCD.
        /// </summary>
        /// <value>
        /// The LCD.
        /// </value>
        public (Hd44780LcdDevice Device, Hd44780LcdDeviceSettings Settings) Lcd => this.lcdDevice.Value;

        /// <summary>
        /// Gets the input controls.
        /// </summary>
        /// <value>
        /// The input controls.
        /// </value>
        public InputControls InputControls => this.inputControls.Value;

        /// <summary>
        /// Gets the music player.
        /// </summary>
        /// <value>
        /// The music player.
        /// </value>
        public MusicPlayer MusicPlayer => this.musicPlayer.Value.MusicPlayer;

        /// <summary>
        /// Gets the volume controls.
        /// </summary>
        /// <value>
        /// The volume controls.
        /// </value>
        public VolumeControls VolumeControls => this.volumeControls.Value;

        /// <summary>
        /// Gets the remote pi.
        /// </summary>
        /// <value>
        /// The remote pi.
        /// </value>
        public RemotePiDevice RemotePi { get; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            this.disposer = new Disposer(
                new GpioConnection(
                    new[]
                    {
                        this.InputControls.PlayPauseButton.PinConfiguration,
                        this.InputControls.NextButton.PinConfiguration,
                        this.InputControls.PreviousButton.PinConfiguration,
                        this.InputControls.MenuButton.PinConfiguration,
                        this.VolumeControls.HeadPhoneSwitch?.PinConfiguration,
                    }.Where(x => x != null)),
                this.InputControls.RemoteControl,
                this.InputControls.RfidTransceiver,
                this.InputControls.RotaryEncoder,
                this.VolumeControls.Amplifier,
                this.lcdDevice.Value.LcdDevice,
                this.musicPlayer.Value.MusicPlayer,
                this.musicPlayer.Value.MpcConnection,
                this.RemotePi);
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.disposer?.Dispose();
        }
    }
}