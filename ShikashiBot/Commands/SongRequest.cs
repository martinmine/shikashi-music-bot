using Discord.Commands;
using ShikashiBot.Services;
using ShikashiBot.Services.YouTube;
using System;
using System.Threading.Tasks;

namespace ShikashiBot.Commands
{
    public class SongRequest : ModuleBase
    {
        public YouTubeDownloadService YoutubeDownloadService { get; set; }
        public SongService SongService { get; set; }

        [Alias("sq", "request", "play")]
        [Command("songrequest", RunMode = RunMode.Async), Summary("Requests a song to be played")]
        public async Task Request([Remainder, Summary("URL of the video to play")] string url)
        {
            try
            {
                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to download {url}");
                DownloadedVideo video = await YoutubeDownloadService.DownloadVideo(url);
                await downloadAnnouncement.DeleteAsync();

                if (video == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to queue song, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                await ReplyAsync($"**{Context.User.Mention} queued {video.Title} | `{TimeSpan.FromSeconds(video.Duration)}` | {url}**");

                SongService.Queue(video);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing song requet: {e}");
            }
        }
    }
}
