using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Serilog;

namespace ShikashiBot.Services
{
    public class SongService
    {
        private IVoiceChannel _voiceChannel;
        private IMessageChannel _messageChannel;
        private BufferBlock<IPlayable> _songQueue;

        public SongService()
        {
            _songQueue = new BufferBlock<IPlayable>();
        }

        public AudioPlaybackService AudioPlaybackService { get; set; }

        public IPlayable NowPlaying { get; private set; }

        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this._voiceChannel = voiceChannel;
            ProcessQueue();
        }

        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            this._messageChannel = messageChannel;
        }

        public void Next()
        {
            AudioPlaybackService.StopCurrentOperation();
        }

        public IList<IPlayable> Clear()
        {
            _songQueue.TryReceiveAll(out var skippedSongs);

            Log.Information($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public void Queue(IPlayable video)
        {
            _songQueue.Post(video);
        }

        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Log.Information("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    await _messageChannel?.SendMessageAsync($"Now playing **{NowPlaying.Title}** | `{NowPlaying.DurationString}` | requested by {NowPlaying.Requester} | {NowPlaying.Url}");

                    Log.Information("Connecting to voice channel");
                    using (var audioClient = await _voiceChannel.ConnectAsync())
                    {
                        Log.Information("Connected!");
                        await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);
                    }

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Log.Information($"Error while playing song: {e}");
                }
            }
        }
    }
}
