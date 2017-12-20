namespace Aupli.CommandLine.Rfid.Mfrc522
{
    using System;
    using System.Threading.Tasks;
    using Pi.IO.GeneralPurpose;
    using Pi.IO.SerialPeripheralInterface;
    using Pi.Timers;

    public sealed class Mfrc522Device : IDisposable
    {
        private IGpioConnectionDriver gpioConnectionDriver;
        private NativeSpiConnection nativeSpiConnection;
        private GpioOutputBinaryPin resetPowerDown;

        public void Initialize(string deviceFilePath, ConnectorPin resetConnectorPin)
        {
            try
            {
                this.gpioConnectionDriver = GpioConnectionSettings.DefaultDriver;
                this.resetPowerDown = this.gpioConnectionDriver.Out(resetConnectorPin);
                this.resetPowerDown.Write(true);
            }
            catch (Exception ex)
            {
                throw new Exception("GPIO initialization failed", ex);
            }

            try
            {
                var settings = new SpiConnectionSettings
                {
                    MaxSpeed = 1000000,
                    Mode = SpiMode.Mode0,
                };

                this.nativeSpiConnection = new NativeSpiConnection(deviceFilePath, settings);
            }/* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                throw new Exception("SPI Initialization Failed", ex);
            }

            this.Reset();
        }

        public void Reset()
        {
            this.resetPowerDown.Write(false);
            Timer.Sleep(TimeSpan.FromMilliseconds(50));
            this.resetPowerDown.Write(true);
            Timer.Sleep(TimeSpan.FromMilliseconds(50));

            // Force 100% ASK modulation
            this.WriteRegister(Registers.TxAsk, 0x40);

            // Set CRC to 0x6363
            this.WriteRegister(Registers.Mode, 0x3D);

            // Enable antenna
            this.SetRegisterBits(Registers.TxControl, 0x03);
        }

        public bool IsTagPresent()
        {
            // Enable short frames
            this.WriteRegister(Registers.BitFraming, 0x07);

            // Transceive the Request command to the tag
            this.Transceive(false, PiccCommands.Request);

            // Disable short frames
            this.WriteRegister(Registers.BitFraming, 0x00);

            // Check if we found a card
            return this.GetFifoLevel() == 2 && this.ReadFromFifoShort() == PiccResponses.AnswerToRequest;
        }

        public Uid ReadUid()
        {
            // Run the anti-collision loop on the card
            this.Transceive(false, PiccCommands.Anticollision1, PiccCommands.Anticollision2);

            // Return tag UID from FIFO
            return new Uid(this.ReadFromFifo(5));
        }

        public void HaltTag()
        {
            // Transceive the Halt command to the tag
            this.Transceive(false, PiccCommands.Halt1, PiccCommands.Halt2);
        }

        public bool SelectTag(Uid uid)
        {
            // Send Select command to tag
            var data = new byte[7];
            data[0] = PiccCommands.Select1;
            data[1] = PiccCommands.Select2;
            uid.FullUid.CopyTo(data, 2);

            this.Transceive(true, data);

            return this.GetFifoLevel() == 1 && this.ReadFromFifo() == PiccResponses.SelectAcknowledge;
        }

        public void Dispose()
        {
            this.nativeSpiConnection?.Dispose();
            this.resetPowerDown?.Dispose();
        }

        internal byte[] ReadBlock(byte blockNumber, Uid uid, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
            {
                this.MifareAuthenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
            }
            else if (keyB != null)
            {
                this.MifareAuthenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
            }
            else
            {
                return null;
            }

            // Read block
            this.Transceive(true, PiccCommands.Read, blockNumber);

            return this.ReadFromFifo(16);
        }

        internal bool WriteBlock(byte blockNumber, Uid uid, byte[] data, byte[] keyA = null, byte[] keyB = null)
        {
            if (keyA != null)
            {
                this.MifareAuthenticate(PiccCommands.AuthenticateKeyA, blockNumber, uid, keyA);
            }
            else if (keyB != null)
            {
                this.MifareAuthenticate(PiccCommands.AuthenticateKeyB, blockNumber, uid, keyB);
            }
            else
            {
                return false;
            }

            // Write block
            this.Transceive(true, PiccCommands.Write, blockNumber);

            if (this.ReadFromFifo() != PiccResponses.Acknowledge)
            {
                return false;
            }

            // Make sure we write only 16 bytes
            var buffer = new byte[16];
            data.CopyTo(buffer, 0);

            this.Transceive(true, buffer);

            return this.ReadFromFifo() == PiccResponses.Acknowledge;
        }

        private void MifareAuthenticate(byte command, byte blockNumber, Uid uid, byte[] key)
        {
            // Put reader in Idle mode
            this.WriteRegister(Registers.Command, PcdCommands.Idle);

            // Clear the FIFO
            this.SetRegisterBits(Registers.FifoLevel, 0x80);

            // Create Authentication packet
            var data = new byte[12];
            data[0] = command;
            data[1] = (byte)(blockNumber & 0xFF);
            key.CopyTo(data, 2);
            uid.Bytes.CopyTo(data, 8);

            this.WriteToFifo(data);

            // Put reader in MfAuthent mode
            this.WriteRegister(Registers.Command, PcdCommands.MifareAuthenticate);

            // Wait for (a generous) 25 ms
            Task.Delay(25).Wait();
        }

        private void Transceive(bool enableCrc, params byte[] data)
        {
            if (enableCrc)
            {
                // Enable CRC
                this.SetRegisterBits(Registers.TxMode, 0x80);
                this.SetRegisterBits(Registers.RxMode, 0x80);
            }

            // Put reader in Idle mode
            this.WriteRegister(Registers.Command, PcdCommands.Idle);

            // Clear the FIFO
            this.SetRegisterBits(Registers.FifoLevel, 0x80);

            // Write the data to the FIFO
            this.WriteToFifo(data);

            // Put reader in Transceive mode and start sending
            this.WriteRegister(Registers.Command, PcdCommands.Transceive);
            this.SetRegisterBits(Registers.BitFraming, 0x80);

            // Wait for (a generous) 25 ms
            Task.Delay(25).Wait();

            // Stop sending
            this.ClearRegisterBits(Registers.BitFraming, 0x80);

            if (enableCrc)
            {
                // Disable CRC
                this.ClearRegisterBits(Registers.TxMode, 0x80);
                this.ClearRegisterBits(Registers.RxMode, 0x80);
            }
        }

        private byte[] ReadFromFifo(int length)
        {
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
            {
                buffer[i] = this.ReadRegister(Registers.FifoData);
            }

            return buffer;
        }

        private byte ReadFromFifo()
        {
            return this.ReadFromFifo(1)[0];
        }

        private void WriteToFifo(params byte[] values)
        {
            foreach (var b in values)
            {
                this.WriteRegister(Registers.FifoData, b);
            }
        }

        private int GetFifoLevel()
        {
            return this.ReadRegister(Registers.FifoLevel);
        }

        private byte ReadRegister(byte register)
        {
            register <<= 1;
            register |= 0x80;

            var writeBuffer = new byte[] { register, 0x00 };

            return this.TransferSpi(writeBuffer)[1];
        }

        private ushort ReadFromFifoShort()
        {
            var low = this.ReadRegister(Registers.FifoData);
            var high = (ushort)(this.ReadRegister(Registers.FifoData) << 8);

            return (ushort)(high | low);
        }

        private void WriteRegister(byte register, byte value)
        {
            register <<= 1;

            var writeBuffer = new[] { register, value };

            this.TransferSpi(writeBuffer);
        }

        private byte[] TransferSpi(byte[] writeBuffer)
        {
            var readBuffer = new byte[writeBuffer.Length];
            using (var transferBuffer =
                this.nativeSpiConnection.CreateTransferBuffer(writeBuffer.Length, SpiTransferMode.ReadWrite))
            {
                transferBuffer.Tx.Copy(writeBuffer, 0, 0, writeBuffer.Length);
                this.nativeSpiConnection.Transfer(transferBuffer);
                transferBuffer.Rx.Copy(0, readBuffer, 0, readBuffer.Length);
            }

            return readBuffer;
        }

        private void SetRegisterBits(byte register, byte bits)
        {
            var currentValue = this.ReadRegister(register);
            this.WriteRegister(register, (byte)(currentValue | bits));
        }

        private void ClearRegisterBits(byte register, byte bits)
        {
            var currentValue = this.ReadRegister(register);
            this.WriteRegister(register, (byte)(currentValue & ~bits));
        }
    }
}
