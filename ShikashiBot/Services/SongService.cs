using ShikashiBot.Services.YouTube;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShikashiBot.Services
{
    public class SongService
    {
        private Queue<DownloadedVideo> _songQueue;

        public SongService()
        {
            this._songQueue = new Queue<DownloadedVideo>();
        }

        public void Queue(DownloadedVideo video)
        {
            _songQueue.Enqueue(video);
        }
    }
}
