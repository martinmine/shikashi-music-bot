using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShikashiBot.Services;
using ShikashiBot.Services.YouTube;
using System;
using System.IO;
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

        public async Task MainAsync()
        {
            const string secretLocation = "Settings/BotSecret.txt";
            if (!File.Exists(secretLocation))
            {
                Directory.CreateDirectory("Settings");
                Console.WriteLine("Enter the bot secret:");
                File.WriteAllText(secretLocation, Console.ReadLine());
                Console.Clear();
            }

            string botSecret = File.ReadAllText(secretLocation);

            _client = new DiscordSocketClient();
            _client.Log += Log;

            _commands = new CommandService();

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _services = serviceCollection.BuildServiceProvider();

            _services.GetService<SongService>().AudioPlaybackService = _services.GetService<AudioPlaybackService>();

            try
            {
                await InstallCommands();

                await _client.LoginAsync(TokenType.User, botSecret);
                await _client.StartAsync();

                _client.GuildAvailable += _client_GuildAvailable;

                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to start the bot because:\n {e}");
                Console.WriteLine("Clear BotSecret.txt? (y/n)");

                if (Console.ReadLine().ToLower() == "y")
                    File.Delete(secretLocation);
            }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new YouTubeDownloadService());
            serviceCollection.AddSingleton(new AudioPlaybackService());
            serviceCollection.AddSingleton(new SongService());
        }

        private Task _client_GuildAvailable(SocketGuild arg)
        {
            if (arg.Name == "1up")
            {
                Console.WriteLine("Registering handler for Shikashi");
                var musicVoiceChannel = arg.GetVoiceChannel(183635996838068226);
                var musicRequestChannel = arg.TextChannels.Where(t => t.Name.ToLower().Contains("music")).SingleOrDefault();

                _services.GetService<SongService>().SetVoiceChannel(musicVoiceChannel);
                _services.GetService<SongService>().SetMessageChannel(musicRequestChannel);
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
            int count = _commands.Commands.Count();
            if (count <= 1)
            {
                throw new Exception("Not enough commands");
            }
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!', a mention prefix, or an url.
            if (!Uri.IsWellFormedUriString(message.Content, UriKind.Absolute) 
                && !message.HasCharPrefix('!', ref argPos) 
                && !message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            if (!message.Channel.Name.ToLower().Contains("music"))
            {
                return;
            }

            // Create a Command Context
            var context = new CommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            IResult result;
            if (Uri.IsWellFormedUriString(message.Content, UriKind.Absolute))
            {
                result = await _commands.ExecuteAsync(context, "sq " + message.Content, _services);
            }
            else
            {
                result = await _commands.ExecuteAsync(context, argPos, _services);
            }

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
