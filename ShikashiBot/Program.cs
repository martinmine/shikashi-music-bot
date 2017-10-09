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

            // Block until someone types exit in the console
            while (Console.ReadLine() != "exit") ;
        }
    }
}
