using System;
using System.IO;
using Newtonsoft.Json;

namespace ShikashiBot.Services.YouTube
{
    /// <summary>
    /// Represents a downloaded video.
    /// </summary>
    public class DownloadedVideo : IPlayable
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
        [JsonProperty(PropertyName = "_filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Person requesting the download
        /// </summary>
        public string Requester { get; set; }

        public string DurationString => TimeSpan.FromSeconds(Duration).ToString();

        public string Uri => FileName;

        /// <summary>
        /// Speed modifier passed to ffmpeg (Frequency)
        /// </summary>
        public int Speed { get; set; } = 48;

        public void OnPostPlay()
        {
            File.Delete(FileName);
        }
    }
}
