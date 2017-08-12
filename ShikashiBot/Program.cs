namespace ShikashiBot
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyHelper.TestDependencies();
            new ShikashiBot().MainAsync().GetAwaiter().GetResult();
        }
    }
}
