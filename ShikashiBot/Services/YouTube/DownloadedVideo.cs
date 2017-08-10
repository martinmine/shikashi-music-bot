using Newtonsoft.Json;

namespace ShikashiBot.Services.YouTube
{
    /// <summary>
    /// Represents a downloaded video.
    /// </summary>
    public class DownloadedVideo
    {
        /// <summary>
        /// Title of the video
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Duration of the video in seconds.
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        /// <summary>
        /// The URL used to access the video site (note: not the actual video itself).
        /// </summary>
        [JsonProperty(PropertyName = "webpage_url")]
        public string Url { get; set; }

        /// <summary>
        /// Unique ID of the video, e.g. YouTube video ID.
        /// </summary>
        [JsonProperty(PropertyName = "display_id")]
        public string DisplayID { get; set; }

        /// <summary>
        /// Name of the file it got stored on.
        /// </summary>
        public string FileName { get; set; }
    }
}
