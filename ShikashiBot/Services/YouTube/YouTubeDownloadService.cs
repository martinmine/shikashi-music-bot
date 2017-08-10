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
            Guid filename = Guid.NewGuid();

            ProcessStartInfo youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "youtube-dl",
                Arguments = $"-o Songs/{filename}.mp3 --extract-audio --no-overwrites --print-json --audio-format mp3 " + url
            };

            Console.WriteLine($"Starting download: {youtubeDlStartupInfo.Arguments}");
            Process youtubeDl = Process.Start(youtubeDlStartupInfo);
            string jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Console.WriteLine($"Download completed with exit code {youtubeDl.ExitCode}");

            DownloadedVideo video = JsonConvert.DeserializeObject<DownloadedVideo>(jsonOutput);

            if (video != null)
            {
                video.FileName = $"Songs/{filename}.mp3";
            }
            
            return video;
        }
    }
}
