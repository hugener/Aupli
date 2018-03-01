// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Uid.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.RfidTransceivers.Mfrc522
{
    /// <summary>
    /// Represents the unique id of a NFC tag.
    /// </summary>
    public class Uid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Uid"/> class.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <param name="third">The third.</param>
        /// <param name="fourth">The fourth.</param>
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

        /// <summary>
        /// Gets the BCC.
        /// </summary>
        /// <value>
        /// The BCC.
        /// </value>
        public byte Bcc { get; }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public byte[] Bytes { get; }

        /// <summary>
        /// Gets the full uid.
        /// </summary>
        /// <value>
        /// The full uid.
        /// </value>
        public byte[] FullUid { get; }

        /// <summary>
        /// Gets a value indicating whether the Uid is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public sealed override int GetHashCode()
        {
            int uid = 0;

            for (int i = 0; i < 4; i++)
            {
                uid |= this.Bytes[i] << (i * 8);
            }

            return uid;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public sealed override string ToString()
        {
            var formatString = "x" + (this.Bytes.Length * 2);
            return this.GetHashCode().ToString(formatString);
        }
    }
}
