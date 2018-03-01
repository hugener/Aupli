// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ky040Connection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.Encoders.Ky040
{
    using System;
    using global::Pi.IO.GeneralPurpose;

    /// <inheritdoc />
    /// <summary>
    /// Pin connection to a KY-040 rotary encoder.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class Ky040Connection : IDisposable
    {
        private const int FullTurnSwitchesState = 0b0011;
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly ProcessorPin clockProcessorPin;
        private readonly ProcessorPin dataProcessorPin;
        private readonly GpioConnection gpioConnection;

        private int lastSwitchesState = FullTurnSwitchesState;
        private int encoderValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ky040Connection" /> class.
        /// </summary>
        /// <param name="gpioConnectionDriver">The gpio connection driver.</param>
        /// <param name="clockConnectorPin">The clock connector pin.</param>
        /// <param name="dataConnectorPin">The data connector pin.</param>
        /// <param name="buttonConnectorPin">The button connector pin.</param>
        public Ky040Connection(IGpioConnectionDriver gpioConnectionDriver, ConnectorPin clockConnectorPin, ConnectorPin dataConnectorPin, ConnectorPin buttonConnectorPin)
        {
            this.gpioConnectionDriver = gpioConnectionDriver;
            var clkPinConfiguration = clockConnectorPin.Input().PullUp();
            clkPinConfiguration.OnStatusChanged(this.OnEncoderChanged);
            var dtPinConfiguration = dataConnectorPin.Input().PullUp();
            dtPinConfiguration.OnStatusChanged(this.OnEncoderChanged);
            var buttonPinConfiguration = buttonConnectorPin.Input().PullUp();
            buttonPinConfiguration.OnStatusChanged(this.OnButtonPressed);
            this.clockProcessorPin = clockConnectorPin.ToProcessor();
            this.dataProcessorPin = dataConnectorPin.ToProcessor();
            this.gpioConnection = new GpioConnection(new GpioConnectionSettings { PollInterval = TimeSpan.FromMilliseconds(5) }, buttonPinConfiguration, clkPinConfiguration, dtPinConfiguration);
            if ((this.gpioConnectionDriver.GetCapabilities() & GpioConnectionDriverCapabilities.CanSetPinDetectedEdges) > 0)
            {
                this.gpioConnectionDriver.SetPinDetectedEdges(this.clockProcessorPin, PinDetectedEdges.Both);
                this.gpioConnectionDriver.SetPinResistor(this.clockProcessorPin, PinResistor.PullUp);

                this.gpioConnectionDriver.SetPinDetectedEdges(this.dataProcessorPin, PinDetectedEdges.Both);
                this.gpioConnectionDriver.SetPinResistor(this.dataProcessorPin, PinResistor.PullUp);
            }
        }

        /// <summary>
        /// Occurs when the encoder is rotated.
        /// </summary>
        public event EventHandler<RotationEventArgs> Rotated;

        /// <summary>
        /// Occurs when the encoder is pressed.
        /// </summary>
        public event EventHandler Pressed;

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.gpioConnection.Dispose();
        }

        private void OnEncoderChanged(bool obj)
        {
            var lsb = this.gpioConnectionDriver.Read(this.clockProcessorPin) ? 1 : 0;
            var msb = this.gpioConnectionDriver.Read(this.dataProcessorPin) ? 1 : 0;

            var switchesState = msb << 1 | lsb;
            var encoderState = (EncoderStates)(switchesState << 2 | this.lastSwitchesState);
            switch (encoderState)
            {
                case EncoderStates.ClockFallingEdgeAndDataIsHigh:
                case EncoderStates.DataFallingEdgeAndClockIsLow:
                case EncoderStates.ClockRisingEdgeAndDataIsLow:
                case EncoderStates.DataRisingEdgeAndClockIsHigh:
                    this.encoderValue++;
                    break;
                case EncoderStates.DataFallingEdgeAndClockIsHigh:
                case EncoderStates.ClockFallingEdgeAndDataIsLow:
                case EncoderStates.DataRisingEdgeAndClockIsLow:
                case EncoderStates.ClockRisingEdgeAndDataIsHigh:
                    this.encoderValue--;
                    break;
            }

            Console.WriteLine(encoderState);
            if (switchesState == FullTurnSwitchesState)
            {
                switch (this.encoderValue >> 2)
                {
                    case 1:
                        this.Rotated?.Invoke(this, new RotationEventArgs(EncoderDirection.Clockwise));
                        this.encoderValue = 0;
                        break;
                    case -1:
                        this.Rotated?.Invoke(this, new RotationEventArgs(EncoderDirection.CounterClockwise));
                        this.encoderValue = 0;
                        break;
                }
            }

            this.lastSwitchesState = switchesState;
        }

        private void OnButtonPressed(bool state)
        {
            if (!state)
            {
                this.Pressed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
