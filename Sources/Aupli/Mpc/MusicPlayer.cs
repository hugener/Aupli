// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicPlayer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Mpc
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.Numeric;
    using MpcNET;
    using Sundew.Base.Threading;

    /// <summary>
    /// Aupli music player.
    /// </summary>
    public class MusicPlayer : IMusicPlayer
    {
        private readonly AsyncLock mpcCommandLock = new AsyncLock();
        private readonly MpcConnection mpcConnection;
        private readonly IMusicPlayerObserver musicPlayerObserver;
        private readonly Task musicPlayerTask;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private string currentPlaylist;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayer" /> class.
        /// </summary>
        /// <param name="mpcConnection">The MPC connection.</param>
        /// <param name="musicPlayerObserver">The music player observer.</param>
        public MusicPlayer(MpcConnection mpcConnection, IMusicPlayerObserver musicPlayerObserver)
        {
            this.mpcConnection = mpcConnection;
            this.musicPlayerObserver = musicPlayerObserver;
            this.musicPlayerTask = new TaskFactory().StartNew(
                this.GetStatus,
                this.cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        /// <summary>
        /// Occurs when status has changed.
        /// </summary>
        public event EventHandler<StatusEventArgs> StatusChanged;

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public PlayerStatus Status { get; private set; } =
            new PlayerStatus(string.Empty, string.Empty, PlayerState.Unknown, -1, TimeSpan.Zero);

        /// <summary>
        /// Updates the status asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task UpdateStatusAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.UpdateDisplayWithCurrentSongAsync();
            });
        }

        /// <summary>
        /// Updates the databse asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task UpdateAsync()
        {
            await this.ExecuteCommandAsync(
                async () => { await this.mpcConnection.SendAsync(commands => commands.Database.Update()); });
        }

        /// <summary>
        /// Sets the volume asynchronously.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <returns>An async task.</returns>
        public async Task SetVolumeAsync(Percentage volume)
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.mpcConnection.SendAsync(commands => commands.Playback.SetVolume((byte)(volume.Value * 100)));
            });
        }

        /// <summary>
        /// Plays the playlist asynchronous.
        /// </summary>
        /// <param name="playlistName">Name of the playlist.</param>
        /// <returns>An async task.</returns>
        public async Task PlayPlaylistAsync(string playlistName)
        {
            await this.ExecuteCommandAsync(async () =>
            {
                if (playlistName != null && playlistName != this.currentPlaylist)
                {
                    this.musicPlayerObserver.StartingPlaylist(playlistName);
                    await this.mpcConnection.SendAsync(commands => commands.Playback.Stop());
                    await this.mpcConnection.SendAsync(commands => commands.CurrentPlaylist.Clear());
                    var loadResult = await this.mpcConnection.SendAsync(commands => commands.StoredPlaylist.Load(playlistName));
                    if (loadResult.IsResponseValid)
                    {
                        this.currentPlaylist = playlistName;
                        await this.mpcConnection.SendAsync(commands => commands.Playback.Play(0));
                        await Task.Delay(10);
                        await this.UpdateDisplayWithCurrentSongAsync();
                        return;
                    }

                    this.currentPlaylist = string.Empty;
                }
                else
                {
                    this.musicPlayerObserver.IgnoredPlaylist(playlistName);
                }
            });
        }

        /// <summary>
        /// Plays or pauses asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task PlayPauseAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.mpcConnection.SendAsync(commands => commands.Playback.PlayPause());
                await Task.Delay(10);
                await this.UpdateDisplayWithCurrentSongAsync();
            });
        }

        /// <summary>
        /// Plays the next song asynchronously.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task NextAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.mpcConnection.SendAsync(commands => commands.Playback.Next());
                await Task.Delay(10);
                await this.UpdateDisplayWithCurrentSongAsync();
            });
        }

        /// <summary>
        /// Plays the previous song asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task PreviousAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.mpcConnection.SendAsync(commands => commands.Playback.Previous());
                await Task.Delay(10);
                await this.UpdateDisplayWithCurrentSongAsync();
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.cancellationTokenSource?.Cancel();
            this.musicPlayerTask?.Wait();
            this.cancellationTokenSource?.Dispose();
        }

        private static PlayerState GetPlayerState(MpdState mpdState)
        {
            switch (mpdState)
            {
                case MpdState.Play:
                    return PlayerState.Playing;
                case MpdState.Stop:
                    return PlayerState.Stopped;
                case MpdState.Pause:
                    return PlayerState.Paused;
                default:
                    return PlayerState.Unknown;
            }
        }

        private async Task UpdateDisplayWithCurrentSongAsync()
        {
            var currentSongResult = await this.mpcConnection.SendAsync(commands => commands.Status.GetCurrentSong());
            var statusResult = await this.mpcConnection.SendAsync(commands => commands.Status.GetStatus());
            if (currentSongResult.IsResponseValid && statusResult.IsResponseValid)
            {
                var currentSong = currentSongResult.Response.Content;
                var status = statusResult.Response.Content;
                if (currentSong != null && status != null)
                {
                    var playerStatus = new PlayerStatus(
                        currentSong.Artist,
                        currentSong.Title,
                        GetPlayerState(status.State),
                        currentSong.Position,
                        TimeSpan.FromSeconds(Math.Round(status.Elapsed.TotalSeconds, 0)));
                    if (!playerStatus.Equals(this.Status))
                    {
                        this.Status = playerStatus;
                        this.StatusChanged?.Invoke(this, new StatusEventArgs(this.Status));
                    }
                }
            }
        }

        private async Task ExecuteCommandAsync(Func<Task> action)
        {
            // await this.mpcConnection.SendAsync(Command.Status.NoIdle());
            using (var lockResult = await this.mpcCommandLock.WaitAsync())
            {
                if (lockResult)
                {
                    await action();
                }
            }
        }

        private async void GetStatus()
        {
            try
            {
                var cancellationToken = this.cancellationTokenSource.Token;
                while (!cancellationToken.IsCancellationRequested)
                {
                    using (var lockResult = await this.mpcCommandLock.WaitAsync(cancellationToken))
                    {
                        if (lockResult)
                        {
                            using (this.musicPlayerObserver.EnterStatusRefresh())
                            {
                                await this.UpdateDisplayWithCurrentSongAsync();
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
                    }

                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
