namespace SnakeGameApp
{
    class Program
    {
        public static async Task Main()
        {
            var game = new SnakeGame();
            await game.RunAsync();
        }
    }
}