using System;
using System.Runtime.InteropServices;
using Serilog;

namespace ShikashiBot
{
    internal class DependencyHelper
    {
        public static void TestDependencies()
        {
            var opusVersion = Marshal.PtrToStringAnsi(OpusVersionString());
            Log.Information($"Loaded opus with version string: {opusVersion}");
            var sodiumVersion = Marshal.PtrToStringAnsi(SodiumVersionString());
            Log.Information($"Loaded sodium with version string: {sodiumVersion}");
        }

        [DllImport("opus", EntryPoint = "opus_get_version_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OpusVersionString();

        [DllImport("libsodium", EntryPoint = "sodium_version_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SodiumVersionString();
    }
}
