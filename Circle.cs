
namespace SnakeGameApp
{
    public class Circle : AbstractGraphic2D
    {
        public decimal CenterX {get;set;}
        public decimal CenterY {get;set;}
        public decimal Radius {get;set;}
        public override decimal LowerBoundX {get; protected set;}

        public override decimal UpperBoundX {get; protected set;}

        public override decimal LowerBoundY {get; protected set;}

        public override decimal UpperBoundY {get; protected set;}
        public Circle(decimal x, decimal y, decimal r)
        {
            CenterX = x;
            CenterY = y;
            Radius = r;
            if(CenterX - r < 0)
                LowerBoundX = 0;
            else 
                LowerBoundX = CenterX - r;
            UpperBoundX = CenterX + r;
            if(CenterY - r < 0)
                LowerBoundY = 0;
            else 
                LowerBoundY = CenterY - r;
            UpperBoundY = CenterY + r;
        }
        public override bool ContainsPoint(decimal x, decimal y)
        {
            return (Math.Sqrt((double)((x-CenterX)*(x-CenterX) + (y-CenterY)*(y-CenterY))) <= (double)Radius);
        }
    }
}