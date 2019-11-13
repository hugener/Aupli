// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MusicPlayer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.MusicControl
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Player.Ari;
    using Aupli.ApplicationServices.Volume.Ari;
    using Aupli.SystemBoundaries.Bridges.MusicControl;
    using Aupli.SystemBoundaries.MusicControl.Ari;
    using MpcNET;
    using MpcNET.Commands.Database;
    using MpcNET.Commands.Playback;
    using MpcNET.Commands.Playlist;
    using MpcNET.Commands.Status;
    using Sundew.Base.Equality;
    using Sundew.Base.Numeric;
    using Sundew.Base.Threading;
    using Sundew.Base.Threading.Jobs;

    /// <summary>
    /// Aupli music player.
    /// </summary>
    public class MusicPlayer : IMusicPlayer
    {
        private static readonly TimeSpan DefaultCommandDelay = TimeSpan.FromMilliseconds(5);
        private readonly AsyncLock mpcCommandLock = new AsyncLock();
        private readonly AutoResetEventAsync updateStatusSleepEvent = new AutoResetEventAsync(false);
        private readonly AutoResetEventAsync statusUpdatedEvent = new AutoResetEventAsync(false);
        private readonly IMpcConnection mpcConnection;
        private readonly IMusicPlayerReporter? musicPlayerReporter;
        private readonly ContinuousJob musicPlayerStatusJob;

        private string currentPlaylist = string.Empty;

        private MpdStatus lastMpdStatus = new MpdStatus(-1, false, false, false, false, 0, 0, 0, MpdState.Unknown, 0, 0, 0, 0, TimeSpan.Zero, TimeSpan.Zero, 0, 0, 0, 0, 0, string.Empty);
        private Percentage volume;
        private bool isMuted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlayer" /> class.
        /// </summary>
        /// <param name="mpcConnection">The MPC connection.</param>
        /// <param name="musicPlayerReporter">The music player observer.</param>
        public MusicPlayer(IMpcConnection mpcConnection, IMusicPlayerReporter? musicPlayerReporter)
        {
            this.mpcConnection = mpcConnection;
            this.musicPlayerReporter = musicPlayerReporter;
            this.musicPlayerReporter?.SetSource(this);
            this.musicPlayerStatusJob = new ContinuousJob(this.GetStatus, e => musicPlayerReporter?.OnStatusException(e));
        }

        /// <summary>
        /// Occurs when status has changed.
        /// </summary>
        public event EventHandler<StatusEventArgs>? StatusChanged;

        /// <summary>
        /// Occurs when [volume changed].
        /// </summary>
        public event EventHandler<VolumeChangedEventArgs>? VolumeChanged;

        /// <summary>
        /// Occurs when [audio output status changed].
        /// </summary>
        public event EventHandler? AudioOutputStatusChanged;

        /// <summary>
        /// Gets or sets the status automatic refresh period.
        /// </summary>
        /// <value>
        /// The status automatic refresh period.
        /// </value>
        public TimeSpan StatusAutoRefreshPeriod { get; set; } = TimeSpan.FromMilliseconds(1000);

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
        public bool IsAudioOutputActive => this.Status.State == PlayerState.Playing;

        /// <summary>
        /// Updates the status asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task UpdateStatusAsync()
        {
            if (!this.musicPlayerStatusJob.Start())
            {
                this.statusUpdatedEvent.Reset();
                this.updateStatusSleepEvent.Set();
                await this.statusUpdatedEvent.WaitAsync();
            }
        }

        /// <summary>
        /// Updates the database asynchronously.
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
        public async Task<bool> SetVolumeAsync(Percentage volume)
        {
            return await this.ExecuteCommandAsync(async () =>
            {
                if (this.volume != volume)
                {
                    var result = await this.mpcConnection.SendAsync(new SetVolumeCommand((byte)(volume.Value * 100)));
                    this.updateStatusSleepEvent.Set();
                    return result.IsResponseValid;
                }

                return false;
            });
        }

        /// <summary>
        /// Sets the state of the mute.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        public async Task MuteAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                if (!this.isMuted)
                {
                    await this.mpcConnection.SendAsync(new SetVolumeCommand(0));
                    this.updateStatusSleepEvent.Set();
                }
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
                if (playlistName != null)
                {
                    this.musicPlayerReporter?.StartingPlaylist(playlistName);
                    await this.mpcConnection.SendAsync(new StopCommand());
                    await this.mpcConnection.SendAsync(new ClearCommand());
                    var loadResult = await this.mpcConnection.SendAsync(new LoadCommand(playlistName));
                    if (loadResult.IsResponseValid)
                    {
                        this.currentPlaylist = playlistName;
                        await this.mpcConnection.SendAsync(new PlayCommand(0));
                        this.updateStatusSleepEvent.Set();
                        return;
                    }

                    this.currentPlaylist = string.Empty;
                }
            });
        }

        /// <summary>
        /// Pauses or resumes asynchronously.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task PauseResumeAsync()
        {
            await this.ExecuteCommandAsync(async () =>
            {
                await this.mpcConnection.SendAsync(new PauseResumeCommand(this.lastMpdStatus.State == MpdState.Play));
                this.updateStatusSleepEvent.Set();
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
                if (!this.lastMpdStatus.Repeat && this.lastMpdStatus.Song == this.lastMpdStatus.PlaylistLength - 1)
                {
                    await this.mpcConnection.SendAsync(new PlayCommand(0));
                }
                else
                {
                    await this.mpcConnection.SendAsync(new NextCommand());
                }

                this.updateStatusSleepEvent.Set();
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
                if (!this.lastMpdStatus.Repeat && this.lastMpdStatus.Song == 0)
                {
                    await this.mpcConnection.SendAsync(new PlayCommand(this.lastMpdStatus.PlaylistLength - 1));
                }
                else
                {
                    await this.mpcConnection.SendAsync(new PreviousCommand());
                }

                this.updateStatusSleepEvent.Set();
            });
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.musicPlayerStatusJob.Dispose();
            this.mpcCommandLock.Dispose();
        }

        private static PlayerState GetPlayerState(MpdState mpdState)
        {
            return mpdState switch
            {
                MpdState.Play => PlayerState.Playing,
                MpdState.Stop => PlayerState.Stopped,
                MpdState.Pause => PlayerState.Paused,
                _ => PlayerState.Unknown
            };
        }

        private async Task<MusicPlayerEventArgs> ExecuteCurrentSongAndStatusCommandAsync()
        {
            var statusResult = await this.mpcConnection.SendAsync(new StatusCommand());
            var currentSongResult = await this.mpcConnection.SendAsync(new CurrentSongCommand());
            if (currentSongResult.IsResponseValid && statusResult.IsResponseValid)
            {
                var currentSong = currentSongResult.Response.Content;
                var status = statusResult.Response.Content;
                StatusEventArgs? statusEventArgs = null;
                VolumeChangedEventArgs? volumeChangedEventArgs = null;
                EventArgs? audioOutputEventArgs = null;
                if (status != null)
                {
                    this.lastMpdStatus = status;
                    if (currentSong != null)
                    {
                        var playerStatus = new PlayerStatus(
                            this.currentPlaylist,
                            currentSong.Artist,
                            currentSong.Title,
                            GetPlayerState(status.State),
                            currentSong.Position,
                            TimeSpan.FromSeconds(Math.Round(status.Elapsed.TotalSeconds, 0)));
                        if (!playerStatus.Equals(this.Status))
                        {
                            statusEventArgs = new StatusEventArgs(playerStatus);
                            audioOutputEventArgs = playerStatus.State != this.Status.State ? EventArgs.Empty : null;
                            this.Status = playerStatus;
                        }
                    }

                    var newVolume = new Percentage(status.Volume / 100d);
                    var newIsMuted = status.Volume == 0;
                    if (this.volume != newVolume || this.isMuted != newIsMuted)
                    {
                        this.isMuted = newIsMuted;
                        this.volume = newVolume;
                        volumeChangedEventArgs = new VolumeChangedEventArgs(this.volume, this.isMuted);
                    }
                }

                return new MusicPlayerEventArgs(statusEventArgs, volumeChangedEventArgs, audioOutputEventArgs);
            }

            return MusicPlayerEventArgs.EmptyEventArgs;
        }

#nullable disable
        private async Task<TResult> ExecuteCommandAsync<TResult>(Func<Task<TResult>> func)
        {
            using (var lockResult = await this.mpcCommandLock.TryLockAsync())
            {
                if (lockResult)
                {
                    await func();
                }
            }

            return default;
        }
#nullable enable

        private async Task ExecuteCommandAsync(Func<Task> action)
        {
            using var lockResult = await this.mpcCommandLock.TryLockAsync();
            if (lockResult)
            {
                await action();
            }
        }

        private async Task GetStatus(CancellationToken cancellationToken)
        {
            var musicPlayerEventArgs = MusicPlayerEventArgs.EmptyEventArgs;
            using (var lockResult = await this.mpcCommandLock.TryLockAsync(cancellationToken))
            {
                if (lockResult)
                {
                    using (this.musicPlayerReporter?.EnterStatusRefresh())
                    {
                        musicPlayerEventArgs = await this.ExecuteCurrentSongAndStatusCommandAsync();
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }
            }

            if (musicPlayerEventArgs.VolumeChangedEventArgs != null)
            {
                this.VolumeChanged?.Invoke(this, musicPlayerEventArgs.VolumeChangedEventArgs);
            }

            if (musicPlayerEventArgs.AudioOutputEventArgs != null)
            {
                this.AudioOutputStatusChanged?.Invoke(this, musicPlayerEventArgs.AudioOutputEventArgs);
            }

            if (musicPlayerEventArgs.StatusEventArgs != null)
            {
                this.StatusChanged?.Invoke(this, musicPlayerEventArgs.StatusEventArgs);
            }

            if (!musicPlayerEventArgs.Equals(MusicPlayerEventArgs.EmptyEventArgs))
            {
                this.statusUpdatedEvent.Set();
            }

            if (await this.updateStatusSleepEvent.WaitAsync(this.StatusAutoRefreshPeriod, cancellationToken))
            {
                await Task.Delay(DefaultCommandDelay, cancellationToken);
            }
        }

        private class MusicPlayerEventArgs : IEquatable<MusicPlayerEventArgs>
        {
            public static readonly MusicPlayerEventArgs EmptyEventArgs = new MusicPlayerEventArgs(null, null, null);

            public MusicPlayerEventArgs(StatusEventArgs? statusEventArgs, VolumeChangedEventArgs? volumeChangedEventArgs, EventArgs? audioOutputEventArgs)
            {
                this.StatusEventArgs = statusEventArgs;
                this.VolumeChangedEventArgs = volumeChangedEventArgs;
                this.AudioOutputEventArgs = audioOutputEventArgs;
            }

            public StatusEventArgs? StatusEventArgs { get; }

            public VolumeChangedEventArgs? VolumeChangedEventArgs { get; }

            public EventArgs? AudioOutputEventArgs { get; }

            public bool Equals(MusicPlayerEventArgs other)
            {
                return EqualityHelper.Equals(
                    this,
                    other,
                    rhs => this.StatusEventArgs == rhs.StatusEventArgs &&
                           this.VolumeChangedEventArgs == rhs.VolumeChangedEventArgs &&
                           this.AudioOutputEventArgs == rhs.AudioOutputEventArgs);
            }
        }
    }
}
