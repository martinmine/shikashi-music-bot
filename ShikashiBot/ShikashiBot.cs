using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ShikashiBot.Services;
using ShikashiBot.Services.YouTube;

namespace ShikashiBot
{
    internal class ShikashiBot
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task StartAsync()
        {
            const string secretLocation = "Settings/BotSecret.txt";

            try
            {
                if (!File.Exists(secretLocation))
                {
                    Directory.CreateDirectory("Settings");
                    Console.WriteLine("Enter the bot secret:");
                    File.WriteAllText(secretLocation, Console.ReadLine());
                    Console.Clear();
                }

                var botSecret = File.ReadAllText(secretLocation);

                _client = new DiscordSocketClient();
                _client.Log += LogT;

                _commands = new CommandService();

                IServiceCollection serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                _services = serviceCollection.BuildServiceProvider();

                _services.GetService<SongService>().AudioPlaybackService = _services.GetService<AudioPlaybackService>();

                await InstallCommands();

                await _client.LoginAsync(TokenType.Bot, botSecret);
                await _client.StartAsync();

                _client.GuildAvailable += OnClientGuildAvailable;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to start the bot because:\n {e}");
                Console.WriteLine("Clear BotSecret.txt? (y/n)");

                if (Console.ReadLine().ToLower() == "y")
                {
                    File.Delete(secretLocation);
                }
            }
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommand;

            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            var count = _commands.Commands.Count();
            if (count <= 1)
            {
                throw new Exception("Not enough commands");
            }
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null)
            {
                return;
            }

            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;

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
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task LogT(LogMessage msg)
        {
            Log.Information(msg.Message);
            return Task.CompletedTask;
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new YouTubeDownloadService());
            serviceCollection.AddSingleton(new AudioPlaybackService());
            serviceCollection.AddSingleton(new SongService());
        }

        private Task OnClientGuildAvailable(SocketGuild arg)
        {
            if (arg.Name.ToLower() == Environment.GetEnvironmentVariable("SERVER_NAME"))
            {
                Log.Information($"Registering handler for {arg.Name}");
                var musicVoiceChannel = arg.VoiceChannels.Where(t => t.Name.ToLower().Contains("general")).SingleOrDefault();
                var musicRequestChannel = arg.TextChannels.Where(t => t.Name.ToLower().Contains("music")).SingleOrDefault();

                _services.GetService<SongService>().SetVoiceChannel(musicVoiceChannel);
                _services.GetService<SongService>().SetMessageChannel(musicRequestChannel);
            }

            Log.Information($"Discovered server {arg.Name}");
            return Task.CompletedTask;
        }
    }
}
