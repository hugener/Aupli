namespace Aupli.CommandLine.Rfid.Mfrc522
{
    public class Uid
    {
        public Uid(byte first, byte second, byte third, byte fourth)
            : this(new[] { first, second, third, fourth })
        {
        }

        internal Uid(byte[] uid)
        {
            this.FullUid = uid;
            this.Bcc = uid[4];

            this.Bytes = new byte[4];
            System.Array.Copy(this.FullUid, 0, this.Bytes, 0, 4);

            foreach (var b in this.Bytes)
            {
                if (b != 0x00)
                {
                    this.IsValid = true;
                }
            }
        }

        public byte Bcc { get; }

        public byte[] Bytes { get; }

        public byte[] FullUid { get; }

        public bool IsValid { get; }

        public sealed override bool Equals(object obj)
        {
            if (!(obj is Uid))
            {
                return false;
            }

            var uidWrapper = (Uid)obj;

            for (int i = 0; i < 5; i++)
            {
                if (this.FullUid[i] != uidWrapper.FullUid[i])
                {
                    return false;
                }
            }

            return true;
        }

        public sealed override int GetHashCode()
        {
            int uid = 0;

            for (int i = 0; i < 4; i++)
            {
                uid |= this.Bytes[i] << (i * 8);
            }

            return uid;
        }

        public sealed override string ToString()
        {
            var formatString = "x" + (this.Bytes.Length * 2);
            return this.GetHashCode().ToString(formatString);
        }
    }
}
