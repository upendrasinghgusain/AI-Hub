namespace ChatApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await new Chatter().Run();
        }
    }
}
