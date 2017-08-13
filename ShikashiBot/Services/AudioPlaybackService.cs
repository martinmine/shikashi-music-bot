using Discord.Audio;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShikashiBot.Services
{
    public class AudioPlaybackService
    {
        private Process _currentProcess;

        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Console.WriteLine($"Starting ffmpeg with args {ffmpeg.Arguments}");
            return Process.Start(ffmpeg);
        }

        public async Task SendAsync(IAudioClient client, string path)
        {
            _currentProcess = CreateStream(path);
            var output = _currentProcess.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Music);
            await output.CopyToAsync(discord);
            await discord.FlushAsync().ConfigureAwait(false);
            Console.WriteLine($"ffmpeg exited with code {_currentProcess.ExitCode}");
            File.Delete(path);
        }

        public void StopCurrentOperation()
        {
            _currentProcess?.Kill();
        }
    }
}
