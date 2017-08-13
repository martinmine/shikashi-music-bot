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

        public AudioPlaybackService AudioPlaybackService { get; set; }

        public SongService()
        {
            this._songQueue = new BufferBlock<DownloadedVideo>();
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
                DownloadedVideo video = await _songQueue.ReceiveAsync();
                await AudioPlaybackService.SendAsync(audioClient, video.FileName);
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
            Next();

            return skippedSongs;
        }

        public void Queue(DownloadedVideo video)
        {
            _songQueue.Post(video);
        }
    }
}
