using System.Data;
namespace SnakeGameApp
{
    public class SnakeSegment
    {
        public int GridX { get; set; }
        public int GridY { get; set; }
        public bool IsHead { get; set; }
        public ConsoleColor SegmentColor {get; set;} = ConsoleColor.DarkGreen;

        public Rectangle ToRectangle(Direction dir, ConsoleColor color)
        {
            var rect = new Rectangle(GridX * 4, GridY * 2, 4,2)
            {
                DisplayChar = ' ',
                BackgroundColor = color,
                ForegroundColor = color
            };
            return rect;
        }
    }
}