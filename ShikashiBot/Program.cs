namespace ShikashiBot
{
    class Program
    {
        static void Main(string[] args)
                => new ShikashiBot().MainAsync().GetAwaiter().GetResult();
    }
}
