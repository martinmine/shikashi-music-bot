using Discord.Audio;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShikashiBot.Services
{
    public class AudioPlaybackService
    {
        private Process _currentProcess;

        public async Task SendAsync(IAudioClient client, string path)
        {
            _currentProcess = CreateStream(path);
            var output = _currentProcess.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Music);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
            _currentProcess.WaitForExit();
            Console.WriteLine($"ffmpeg exited with code {_currentProcess.ExitCode}");
        }

        public void StopCurrentOperation()
        {
            _currentProcess?.Kill();
        }

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
    }
}
