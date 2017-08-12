using Discord.Commands;
using ShikashiBot.Services;
using System.Threading.Tasks;

namespace ShikashiBot.Commands
{
    class Clear : ModuleBase
    {
        public SongService SongService { get; set; }

        [Command("clear"), Summary("Clears all songs in queue")]
        public async Task ClearQueue()
        {
            SongService.Clear();
            await ReplyAsync("Queue cleared");
        }
    }
}
