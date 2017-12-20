namespace Aupli.CommandLine.Rfid.Mfrc522
{
    using System;

    public class TagDetectedEventArgs : EventArgs
    {
        public TagDetectedEventArgs(Uid uid)
        {
            this.Uid = uid;
        }

        public Uid Uid { get; }
    }
}