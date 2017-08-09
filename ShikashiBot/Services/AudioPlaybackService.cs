using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ShikashiBot.Services
{
    public class AudioPlaybackService
    {
        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Console.WriteLine($"Starting ffmpeg with args {ffmpeg.Arguments}");
            return Process.Start(ffmpeg);
        }

        public async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
            Console.WriteLine($"ffmpeg exited with code {ffmpeg.ExitCode}");
        }
    }
}
