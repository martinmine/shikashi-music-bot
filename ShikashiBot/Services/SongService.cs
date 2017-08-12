using Discord;
using Discord.Audio;
using ShikashiBot.Services.YouTube;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ShikashiBot.Services
{
    public class SongService
    {
        private IVoiceChannel _voiceChannel;
        private BufferBlock<DownloadedVideo> _songQueue;
        private CancellationTokenSource _tokenSource;

        public AudioPlaybackService AudioPlaybackService { get; set; }

        public SongService()
        {
            this._songQueue = new BufferBlock<DownloadedVideo>();
            this._tokenSource = new CancellationTokenSource();
        }

        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this._voiceChannel = voiceChannel;
            ProcessQueue();
        }

        private async void ProcessQueue()
        {
            IAudioClient audioClient = await _voiceChannel.ConnectAsync();

            while (await _songQueue.OutputAvailableAsync())
            {
                await Task.Run(async () =>  {
                    DownloadedVideo video = await _songQueue.ReceiveAsync();
                    await AudioPlaybackService.SendAsync(audioClient, video.FileName);
                }, _tokenSource.Token);
            }
        }

        public void Next()
        {
            _tokenSource.Cancel();
        }

        public IList<DownloadedVideo> Clear()
        {
            IList<DownloadedVideo> skippedSongs;
            _songQueue.TryReceiveAll(out skippedSongs);
            Next();

            return skippedSongs;
        }

        public void Queue(DownloadedVideo video)
        {
            _songQueue.Post(video);
        }
    }
}
