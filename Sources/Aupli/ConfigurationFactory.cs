// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.Player;
    using Newtonsoft.Json;
    using Sundew.Base.Threading;

    /// <summary>
    /// Factory for various configurations.
    /// </summary>
    public class ConfigurationFactory
    {
        private const string SettingsPath = "settings.json";
        private const string PlaylistMapPath = "playlists.json";

        private readonly AsyncLazy<Settings> settings = new AsyncLazy<Settings>(
            async () =>
        {
            var settings = await File.ReadAllTextAsync(SettingsPath);
            return JsonConvert.DeserializeObject<Settings>(settings);
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly AsyncLazy<Dictionary<string, string>> playlistDictionary = new AsyncLazy<Dictionary<string, string>>(
            async () =>
            {
                var playlistContent = await File.ReadAllTextAsync(PlaylistMapPath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(playlistContent);
            }, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Gets the settings asynchronous.
        /// </summary>
        /// <returns>A get lifecycle configuration task.</returns>
        public async Task<Settings> GetSettingsAsync()
        {
            return await this.settings;
        }

        /// <summary>
        /// Gets the playlist map asynchronous.
        /// </summary>
        /// <returns>A get playlist map task.</returns>
        public async Task<PlaylistMap> GetPlaylistMapAsync()
        {
            return new PlaylistMap(await this.playlistDictionary);
        }

        /// <summary>
        /// Saves the setttings asynchronous.
        /// </summary>
        /// <returns>A save task.</returns>
        public async Task SaveSettingsAsync()
        {
            await File.WriteAllTextAsync(SettingsPath, JsonConvert.SerializeObject(await this.settings));
        }

        /// <summary>
        /// Saves the playlist map asynchronous.
        /// </summary>
        /// <returns>A save task.</returns>
        public async Task SavePlaylistMapAsync()
        {
            await File.WriteAllTextAsync(PlaylistMapPath, JsonConvert.SerializeObject(await this.playlistDictionary));
        }
    }
}