using System.Threading.Tasks;
using Serilog;

namespace ShikashiBot
{
    public class Program
    {
        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(a => a.RollingFile("Logs/log-{Date}.txt"))
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("test");
            DependencyHelper.TestDependencies();
            var bot = new ShikashiBot();
            await bot.StartAsync();

            // Block indefinitely
            await Task.Delay(-1);
        }
    }
}
