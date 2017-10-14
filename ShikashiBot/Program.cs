using System;
using System.Threading.Tasks;

namespace ShikashiBot
{
    class Program
    {
        public static async Task Main()
        {
            DependencyHelper.TestDependencies();
            ShikashiBot bot = new ShikashiBot();
            await bot.StartAsync();

            // Block indefinitely
            await Task.Delay(-1);
        }
    }
}
