
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MpcNet.Tester
{
    public class PlaylistDatabase
    {
        private Dictionary<string, string> playlists = new Dictionary<string, string>();

        public async Task LoadAsync()
        {
            var playlistContent = await File.ReadAllTextAsync("playlists.json");
            this.playlists = JsonConvert.DeserializeObject<Dictionary<string, string>>(playlistContent);
        }

        public string GetPlaylist(string tag)
        {
            return playlists[tag];
        }

        public void AddPlaylist(string tag, string playlist)
        {
            this.playlists.Add(tag, playlist);
        }

        public async Task SaveAsync()
        {
            await File.WriteAllTextAsync("playlists.json", JsonConvert.SerializeObject(this.playlists));
        }
    }
}
