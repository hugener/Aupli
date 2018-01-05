using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MpcNET;
using MpcNET.Commands;
using MpcNET.Tags;

namespace MpcNet.Tester
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var playlistDatabase = new PlaylistDatabase();
            await playlistDatabase.LoadAsync();
            var mpcConnection = new MpcConnection(new IPEndPoint(IPAddress.Loopback, 6600));
            await mpcConnection.ConnectAsync();

            var updateResult = await mpcConnection.SendAsync(Command.Database.Update());
            var stopResult = await mpcConnection.SendAsync(Command.Playback.Stop());
            var clearResult = await mpcConnection.SendAsync(Command.Playlists.Current.Clear());

            var findResult = await mpcConnection.SendAsync(Command.Database.Find(FindTags.Base, $@"""Glemmebogen for børn"""));
            foreach (var mpdFile in findResult.Response.Body)
            {
                var addResult = await mpcConnection.SendAsync(Command.Playlists.Current.AddSong(mpdFile.Path));
            }

            var currentPlaylistResult = await mpcConnection.SendAsync(Command.Playlists.Current.GetAllSongsInfo());
            var firstSong = currentPlaylistResult.Response.Body.FirstOrDefault();
            var playResult = await mpcConnection.SendAsync(Command.Playback.Play(firstSong));
            var currentSongResult = await mpcConnection.SendAsync(Command.Status.GetCurrentSong());
            Console.WriteLine(currentSongResult.Response.Body);
            var statusResult = await mpcConnection.SendAsync(Command.Status.GetStatus());
            await Task.Delay(4000);
            Console.WriteLine(statusResult.Response.Body);
            var nextResult = await mpcConnection.SendAsync(Command.Playback.Next());

            var listPlaylistResult = await mpcConnection.SendAsync(Command.Playlists.Stored.GetAll());
            Console.WriteLine(listPlaylistResult.Response.Body);

            var playlist = playlistDatabase.GetPlaylist("8105505");
            var loadResult = await mpcConnection.SendAsync(Command.Database.Find(FindTags.Base, $@"""{playlist}"""));

            await playlistDatabase.SaveAsync();
        }
    }
}
