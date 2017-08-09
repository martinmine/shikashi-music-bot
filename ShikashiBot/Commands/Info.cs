using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ShikashiBot.Commands
{
    // Create a module with no prefix
    public class Info : ModuleBase
    {
        // ~say hello -> hello
        [Command("sasy"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The video to play")] string url)
        {
            // ReplyAsync is a method on ModuleBase
            await ReplyAsync(url);
        }
    }
}
