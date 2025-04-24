namespace SnakeGameApp
{
    public class Rectangle : AbstractGraphic2D
    {
        public override decimal LowerBoundX {get;protected set;}

        public override decimal UpperBoundX {get;protected set;}

        public override decimal LowerBoundY {get;protected set;}

        public override decimal UpperBoundY {get;protected set;}
        public decimal Left {get; protected set;}
        public decimal Top {get; protected set;}
        public decimal Width {get; protected set;}
        public decimal Height {get; protected set;}

        public Rectangle(decimal left, decimal top, decimal width, decimal height )
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            LowerBoundX = Left;
            LowerBoundY = Top;
            UpperBoundX = Left + Width;
            UpperBoundY = Top + Height;
        }
        public override bool ContainsPoint(decimal x, decimal y)
        {
            return (x <= UpperBoundX && x >= LowerBoundX && y<= UpperBoundY && y>=LowerBoundY);
        }
    }
}