// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicPlayer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Mpc
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Volume.Ari;
    using MpcNET;
    using MpcNET.Commands.Database;
    using MpcNET.Commands.Playback;
    using MpcNET.Commands.Playlist;
    using MpcNET.Commands.Status;
    using Sundew.Base.Numeric;
    using Sundew.Base.Threading;

    /// <summary>
    /// Aupli music player.
    /// </summary>
    public class MusicPlayer : IMusicPlayer
    {
        private static readonly TimeSpan DefaultCommandDelay = TimeSpan.FromMilliseconds(5);
        private readonly AsyncLock mpcCommandLock = new AsyncLock();
        private readonly IMpcConnection mpcConnection;
        private readonly IMusicPlayerReporter musicPlayerReporter;
        private readonly Task musicPlayerTask;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private string currentPlaylist;
        private MpdStatus status;
        private Percentage volume;
        private bool isMuted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayer" /> class.
        /// </summary>
        /// <param name="mpcConnection">The MPC connection.</param>
        /// <param name="musicPlayerReporter">The music player observer.</param>
        public MusicPlayer(IMpcConnection mpcConnection, IMusicPlayerReporter musicPlayerReporter)
        {
            this.mpcConnection = mpcConnection;
            this.musicPlayerReporter = musicPlayerReporter;
            this.musicPlayerReporter?.SetSource(this);
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
        /// Occurs when [volume changed].
        /// </summary>
        public event EventHandler<VolumeChangedEventArgs> VolumeChanged;

        /// <summary>
        /// Occurs when [audio output status changed].
        /// </summary>
        public event EventHandler AudioOutputStatusChanged;

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public PlayerStatus Status { get; private set; } = PlayerStatus.NoStatus;

        /// <summary>
        /// Gets a value indicating whether this instance is outputting audio.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is outputting audio; otherwise, <c>false</c>.
        /// </value>
        public bool IsOutputtingAudio => this.Status.State != PlayerState.Playing;

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
                async () => { await this.mpcConnection.SendAsync(new UpdateCommand()); });
        }

        /// <summary>
        /// Sets the volume asynchronously.
        /// </summary>
        /// <param name="volume">The volume.</param>
        /// <returns>An async task.</returns>
        public async Task SetVolumeAsync(Percentage volume)
        {
            if (this.volume != volume)
            {
                await this.ExecuteCommandAsync(async () =>
                {
                    await this.mpcConnection.SendAsync(new SetVolumeCommand((byte)(volume.Value * 100)));
                    await Task.Delay(DefaultCommandDelay);
                    await this.UpdateDisplayWithCurrentSongAsync();
                });
            }
        }

        /// <summary>
        /// Sets the state of the mute.
        /// </summary>
        /// <param name="isMuted">if set to <c>true</c> [is muted].</param>
        /// <returns>An async task.</returns>
        public async Task SetMuteStateAsync(bool isMuted)
        {
            if (this.isMuted != isMuted)
            {
                await this.ExecuteCommandAsync(async () =>
                {
                    await this.mpcConnection.SendAsync(new SetVolumeCommand(0));
                    await Task.Delay(DefaultCommandDelay);
                    await this.UpdateDisplayWithCurrentSongAsync();
                });
            }
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
                if (playlistName != null)
                {
                    this.musicPlayerReporter.StartingPlaylist(playlistName);
                    await this.mpcConnection.SendAsync(new StopCommand());
                    await this.mpcConnection.SendAsync(new ClearCommand());
                    var loadResult = await this.mpcConnection.SendAsync(new LoadCommand(playlistName));
                    if (loadResult.IsResponseValid)
                    {
                        this.currentPlaylist = playlistName;
                        await this.mpcConnection.SendAsync(new PlayCommand(0));
                        await Task.Delay(DefaultCommandDelay);
                        await this.UpdateDisplayWithCurrentSongAsync();
                        return;
                    }

                    this.currentPlaylist = string.Empty;
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
                await this.mpcConnection.SendAsync(new PlayPauseCommand());
                await Task.Delay(DefaultCommandDelay);
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
                if (!this.status.Repeat && this.status.Song == this.status.PlaylistLength - 1)
                {
                    await this.mpcConnection.SendAsync(new PlayCommand(0));
                }
                else
                {
                    await this.mpcConnection.SendAsync(new NextCommand());
                }

                await Task.Delay(DefaultCommandDelay);
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
                if (!this.status.Repeat && this.status.Song == 0)
                {
                    await this.mpcConnection.SendAsync(new PlayCommand(this.status.PlaylistLength - 1));
                }
                else
                {
                    await this.mpcConnection.SendAsync(new PreviousCommand());
                }

                await Task.Delay(DefaultCommandDelay);
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
            var currentSongResult = await this.mpcConnection.SendAsync(new CurrentSongCommand());
            var statusResult = await this.mpcConnection.SendAsync(new StatusCommand());
            if (currentSongResult.IsResponseValid && statusResult.IsResponseValid)
            {
                var currentSong = currentSongResult.Response.Content;
                this.status = statusResult.Response.Content;
                if (currentSong != null && this.status != null)
                {
                    var playerStatus = new PlayerStatus(
                        this.currentPlaylist,
                        currentSong.Artist,
                        currentSong.Title,
                        GetPlayerState(this.status.State),
                        currentSong.Position,
                        TimeSpan.FromSeconds(Math.Round(this.status.Elapsed.TotalSeconds, 0)));
                    if (!playerStatus.Equals(this.Status))
                    {
                        this.Status = playerStatus;
                        this.StatusChanged?.Invoke(this, new StatusEventArgs(this.Status));
                        this.AudioOutputStatusChanged?.Invoke(this, EventArgs.Empty);
                    }

                    var newVolume = new Percentage(this.status.Volume / 100d);
                    if (this.volume != newVolume)
                    {
                        this.isMuted = this.status.Volume == 0;
                        if (!this.isMuted)
                        {
                            this.volume = newVolume;
                        }

                        this.VolumeChanged?.Invoke(this, new VolumeChangedEventArgs(this.volume, this.isMuted));
                    }
                }
            }
        }

        private async Task ExecuteCommandAsync(Func<Task> action)
        {
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
                            using (this.musicPlayerReporter.EnterStatusRefresh())
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
