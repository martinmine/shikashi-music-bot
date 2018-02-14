using System;
using System.Runtime.InteropServices;

namespace ShikashiBot
{
    internal class DependencyHelper
    {
        public static void TestDependencies()
        {
            string opusVersion = Marshal.PtrToStringAnsi(OpusVersionString());
            Console.WriteLine($"Loaded opus with version string: {opusVersion}");
            string sodiumVersion = Marshal.PtrToStringAnsi(SodiumVersionString());
            Console.WriteLine($"Loaded sodium with version string: {sodiumVersion}");
        }

        [DllImport("opus", EntryPoint = "opus_get_version_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OpusVersionString();

        [DllImport("libsodium", EntryPoint = "sodium_version_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SodiumVersionString();
    }
}
