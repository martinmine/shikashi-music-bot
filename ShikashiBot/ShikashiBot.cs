using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShikashiBot.Services;
using ShikashiBot.Services.YouTube;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ShikashiBot
{
    class ShikashiBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private string _botSecret = Environment.GetEnvironmentVariable("BOT_SECRET");

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            _commands = new CommandService();

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _services = serviceCollection.BuildServiceProvider();

            await InstallCommands();

            await _client.LoginAsync(TokenType.Bot, _botSecret);
            await _client.StartAsync();
            
            _client.GuildAvailable += _client_GuildAvailable;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new YouTubeDownloadService());
            serviceCollection.AddSingleton(new AudioPlaybackService());
            serviceCollection.AddSingleton(new SongService());
        }

        private Task _client_GuildAvailable(SocketGuild arg)
        {
            if (arg.Name == "Shikashi")
            {
                Console.WriteLine("Registering handler for Shikashi");
                var channel = arg.TextChannels.Where(t => t.Name == "dev").SingleOrDefault();

               // channel.SendMessageAsync("Ready for requests!");
            }


            Console.WriteLine($"Joined {arg.Name}");
            return Task.CompletedTask;
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommand;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new CommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[LOG] {msg.Message}");
            return Task.CompletedTask;
        }
    }
}
