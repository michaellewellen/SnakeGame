namespace SnakeGameApp
{
    class Program
    {
        public static async Task Main()
        {
            var game = new SnakeGameApp();
            await game.RunAsync();
        }
    }
}