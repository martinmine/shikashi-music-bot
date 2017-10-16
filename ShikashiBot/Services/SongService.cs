using Discord;
using Discord.Audio;
using ShikashiBot.Services.YouTube;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace ShikashiBot.Services
{
    public class SongService
    {
        private IVoiceChannel _voiceChannel;
        private IMessageChannel _messageChannel;
        private BufferBlock<DownloadedVideo> _songQueue;

        public AudioPlaybackService AudioPlaybackService { get; set; }
        public DownloadedVideo NowPlaying { get; private set; }

        public SongService()
        {
            this._songQueue = new BufferBlock<DownloadedVideo>();
        }

        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this._voiceChannel = voiceChannel;
            ProcessQueue();
        }

        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            this._messageChannel = messageChannel;
        }

        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Console.WriteLine("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    await _messageChannel?.SendMessageAsync($"Now playing **{NowPlaying.Title}** | `{TimeSpan.FromSeconds(NowPlaying.Duration)}` | requested by {NowPlaying.Requester} | {NowPlaying.Url}");

                    Console.WriteLine("Connecting to voice channel");
                    using (IAudioClient audioClient = await _voiceChannel.ConnectAsync())
                    {
                        Console.WriteLine("Connected!");
                        await AudioPlaybackService.SendAsync(audioClient, NowPlaying.FileName);
                    }

                    File.Delete(NowPlaying.FileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while playing song: {e}");
                }
            }
        }

        public void Next()
        {
            AudioPlaybackService.StopCurrentOperation();
        }

        public IList<DownloadedVideo> Clear()
        {
            IList<DownloadedVideo> skippedSongs;
            _songQueue.TryReceiveAll(out skippedSongs);

            Console.WriteLine($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public void Queue(DownloadedVideo video)
        {
            _songQueue.Post(video);
        }
    }
}
