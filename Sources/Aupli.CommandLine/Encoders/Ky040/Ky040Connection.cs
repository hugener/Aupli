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
        private readonly GpioInputBinaryPin pinA;
        private readonly GpioInputBinaryPin pinB;
        private int abOld;
        private EncoderDirection encoderDirection;

        public Ky040Connection(ConnectorPin clkConnectorPin, ConnectorPin dtConnectorPin, ConnectorPin buttonConnectorPin)
        {
            this.gpioConnectionDriver = GpioConnectionSettings.DefaultDriver;
            this.clkPinConfiguration = clkConnectorPin.Input().PullUp();
            this.clkPinConfiguration.OnStatusChanged(this.OnEncoderChanged);
            this.dtPinConfiguration = dtConnectorPin.Input().PullUp();
            this.dtPinConfiguration.OnStatusChanged(this.OnEncoderChanged);
            this.buttonPinConfiguration = buttonConnectorPin.Input().PullUp();
            this.buttonPinConfiguration.OnStatusChanged(this.OnButtonPressed);
            this.pinA = this.gpioConnectionDriver.In(clkConnectorPin, PinResistor.PullUp);
            this.pinB = this.gpioConnectionDriver.In(dtConnectorPin, PinResistor.PullUp);
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
            this.pinA.Dispose();
            this.pinB.Dispose();
            this.gpioConnection.Close();
        }

        private void OnEncoderChanged(bool obj)
        {
            var a = this.pinA.Read() ? 1 : 0;
            var b = this.pinB.Read() ? 1 : 0;

            var abNew = a << 1 | b;
            var criterion = abNew ^ this.abOld;

            Console.WriteLine("Criterion: " + criterion);
            switch (criterion)
            {
                case 1:
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.CounterClockwise));
                    break;
                case 2:
                    this.Rotating?.Invoke(this, new RotationEventArgs(EncoderDirection.Clockwise));
                    break;
            }

            this.abOld = abNew;
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
