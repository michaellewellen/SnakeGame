using NUnit.Framework;
using SnakeGameApp;

namespace SnakeGame.Tests;

public class SnakeTests
{
    private SnakeGame game;

    [SetUp]
    public void Setup()
    {
        game = new SnakeGame();
    }

    [Test]
    public void TestThatRuns()
    {
        Assert.Pass("This test works.");
    }
}
