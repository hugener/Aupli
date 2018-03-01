// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LircConnection.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// Based on: https://github.com/shawty/raspberrypi-csharp-lirc

namespace Sundew.Pi.IO.Components.InfraredReceivers.Lirc
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Connection to a unix lirc socket.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class LircConnection : IDisposable
    {
        private const string LircSocketName = "/var/run/lirc/lircd";
        private CancellationTokenSource cancellationTokenSource;
        private Task receiveTask;

        /// <summary>
        /// Occurs when a lirc command received.
        /// </summary>
        public event EventHandler<LircCommandEventArgs> CommandReceived;

        /// <summary>
        /// Starts listening for commands.
        /// </summary>
        public void StartListening()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.receiveTask = new TaskFactory().StartNew(
                this.Receive,
                this.cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        /// <summary>
        /// Stops listening for commands.
        /// </summary>
        public void Stop()
        {
            this.cancellationTokenSource?.Cancel();
            this.receiveTask?.Wait();
            this.cancellationTokenSource?.Dispose();
        }

        /// <inheritdoc />
        void IDisposable.Dispose()
        {
            this.Stop();
        }

        private static LircKeyCodes GetKeyCode(string keyCodeRaw)
        {
            if (KeyMap.TryGetValue(keyCodeRaw, out var keyCode))
            {
                return keyCode;
            }

            return LircKeyCodes.KeyUnknown;
        }

        private async Task Receive()
        {
            try
            {
                var buffer = new byte[500];
                var cancellationToken = this.cancellationTokenSource.Token;
                using (var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP))
                {
                    var unixEndPoint = UnixEndPoint.Create(LircSocketName);
                    await SocketTaskExtensions.ConnectAsync(socket, unixEndPoint);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (socket.Poll(1000, SelectMode.SelectRead))
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            var size = await socket.ReceiveAsync(buffer, SocketFlags.None);
                            cancellationToken.ThrowIfCancellationRequested();

                            var command = Encoding.ASCII.GetString(buffer, 0, size);
                            var commandParts = command.Split(' ');
                            var keyCodeRaw = commandParts[2];
                            var keyCode = GetKeyCode(keyCodeRaw);
                            this.CommandReceived?.Invoke(this, new LircCommandEventArgs(keyCode, keyCodeRaw, int.Parse(commandParts[1]), commandParts[3]));

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}