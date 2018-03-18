using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;

namespace ShikashiBot.Services.YouTube
{
    public class YouTubeDownloadService
    {
        public async Task<DownloadedVideo> DownloadVideo(string url)
        {
            var filename = Guid.NewGuid();

            var youtubeDl = StartYoutubeDl(
                $"-o Songs/{filename}.mp3 --restrict-filenames --extract-audio --no-overwrites --print-json --audio-format mp3 " +
                url);

            if (youtubeDl == null)
            {
                Log.Information("Error: Unable to start process");
                return null;
            }

            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Log.Information($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<DownloadedVideo>(jsonOutput);
        }

        public async Task<StreamMetadata> GetLivestreamData(string url)
        {
            var youtubeDl = StartYoutubeDl("--print-json --skip-download " + url);
            var jsonOutput = await youtubeDl.StandardOutput.ReadToEndAsync();
            youtubeDl.WaitForExit();
            Log.Information($"Download completed with exit code {youtubeDl.ExitCode}");

            return JsonConvert.DeserializeObject<StreamMetadata>(jsonOutput);
        }

        private static Process StartYoutubeDl(string arguments)
        {
            var youtubeDlStartupInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                FileName = "youtube-dl",
                Arguments = arguments
            };

            Log.Information($"Starting youtube-dl with arguments: {youtubeDlStartupInfo.Arguments}");
            return Process.Start(youtubeDlStartupInfo);
        }
    }
}
