using System.Linq;
using Newtonsoft.Json;

namespace ShikashiBot.Services.YouTube
{
    public class StreamMetadata : IPlayable
    {
        public string Url { get; set; }

        public string Uri => Formats.First().Url;

        public string Requester { get; set; }

        public string DurationString => "Live";

        public int Speed => 48;

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "view_count")]
        public string ViewCount { get; set; }

        [JsonProperty(PropertyName = "formats")]
        public StreamFormatMetadata[] Formats { get; set; }

        public void OnPostPlay()
        {
        }
    }
}
