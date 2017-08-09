using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShikashiBot.Services.YouTube
{
    public class YouTubeDownloadService
    {
        public async Task<DownloadedVideo> DownloadVideo(string url)
        {
            ProcessStartInfo youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "youtube-dl",
                Arguments = "-o Songs/%(title)s.%(ext)s --extract-audio --no-overwrites --print-json --audio-format mp3 " + url
            };

            Console.WriteLine($"Starting download of {url}");
            Process youtubeDl = Process.Start(youtubeDlStartupInfo);
            string jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<DownloadedVideo>(jsonOutput);
        }
    }
}
