using Discord.Commands;
using ShikashiBot.Services;
using System.Threading.Tasks;

namespace ShikashiBot.Commands
{
    class Skip : ModuleBase
    {
        public SongService SongService { get; set; }

        [Command("skip"), Summary("Skips current song")]
        public async Task SkipSong()
        {
            SongService.Next();
            await ReplyAsync("Skipped song");
        }
    }
}
