namespace Pipeline
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new Pipeline().Run();
        }
    }
}
