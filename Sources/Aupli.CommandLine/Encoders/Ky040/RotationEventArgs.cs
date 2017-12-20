namespace Aupli.CommandLine.Encoders.Ky040
{
    using System;

    public class RotationEventArgs : EventArgs
    {
        public RotationEventArgs(EncoderDirection encoderDirection)
        {
            this.EncoderDirection = encoderDirection;
        }

        public EncoderDirection EncoderDirection { get; }
    }
}
