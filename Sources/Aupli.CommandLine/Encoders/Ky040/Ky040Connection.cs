namespace Aupli.CommandLine.Encoders.Ky040
{
    using System;
    using Pi.IO.GeneralPurpose;

    public class Ky040Connection : IDisposable
    {
        private readonly InputPinConfiguration clkPinConfiguration;
        private readonly InputPinConfiguration dtPinConfiguration;
        private readonly InputPinConfiguration buttonPinConfiguration;
        private readonly IGpioConnectionDriver gpioConnectionDriver;
        private readonly GpioConnection gpioConnection;
        private readonly GpioInputBinaryPin clkPin;
        private readonly GpioInputBinaryPin dtPin;
        private byte previousSequence;
        private EncoderDirection encoderDirection;

        public Ky040Connection(ConnectorPin clkConnectorPin, ConnectorPin dtConnectorPin, ConnectorPin buttonConnectorPin)
        {
            this.gpioConnectionDriver = GpioConnectionSettings.DefaultDriver;
            this.clkPinConfiguration = clkConnectorPin.Input().PullUp();
            PinConfigurationExtensionMethods.OnStatusChanged(this.clkPinConfiguration, this.OnEncoderChanged);
            this.dtPinConfiguration = dtConnectorPin.Input().PullUp();
            PinConfigurationExtensionMethods.OnStatusChanged(this.dtPinConfiguration, this.OnEncoderChanged);
            this.buttonPinConfiguration = buttonConnectorPin.Input().PullUp();
            PinConfigurationExtensionMethods.OnStatusChanged(this.buttonPinConfiguration, this.OnButtonPressed);
            this.clkPin = GpioBinaryPinExtensionMethods.In(this.gpioConnectionDriver, clkConnectorPin, PinResistor.PullUp);
            this.dtPin = GpioBinaryPinExtensionMethods.In(this.gpioConnectionDriver, dtConnectorPin, PinResistor.PullUp);
            if ((this.gpioConnectionDriver.GetCapabilities() & GpioConnectionDriverCapabilities.CanSetPinDetectedEdges) > 0)
            {
                this.gpioConnectionDriver.SetPinDetectedEdges(clkConnectorPin.ToProcessor(), PinDetectedEdges.Falling);
                this.gpioConnectionDriver.SetPinDetectedEdges(dtConnectorPin.ToProcessor(), PinDetectedEdges.Falling);
            }

            this.gpioConnection = new GpioConnection(this.buttonPinConfiguration, this.clkPinConfiguration, this.dtPinConfiguration);
        }

        public event EventHandler<RotationEventArgs> Rotating;

        public event EventHandler Pressed;

        public void Dispose()
        {
            this.clkPin.Dispose();
            this.dtPin.Dispose();
            this.gpioConnection.Close();
        }

        private void OnEncoderChanged(bool obj)
        {
            var clkState = (byte)(this.clkPin.Read() ? 1 : 0);
            var dtState = (byte)(this.dtPin.Read() ? 1 : 0);

            var c = clkState ^ dtState;
            var sequence = (byte)(c | dtState << 1);

            var delta = (sequence - this.previousSequence) % 4;
            if (delta == 1)
            {
                if (this.encoderDirection == EncoderDirection.Clockwise)
                {
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.Clockwise));
                }
                else
                {
                    this.encoderDirection = EncoderDirection.Clockwise;
                }
            }
            else if (delta == 2)
            {
                if (this.encoderDirection == EncoderDirection.Clockwise)
                {
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.Clockwise));
                }
                else if (this.encoderDirection == EncoderDirection.CounterClockwise)
                {
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.CounterClockwise));
                }
            }
            else if (delta == 3)
            {
                if (this.encoderDirection == EncoderDirection.CounterClockwise)
                {
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.CounterClockwise));
                }
                else
                {
                    this.encoderDirection = EncoderDirection.CounterClockwise;
                }
            }

            this.previousSequence = sequence;
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
