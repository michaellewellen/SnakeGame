namespace SnakeGameApp
{
    public class Ellipse : AbstractGraphic2D
    {
        public decimal CenterX { get; set; }
        public decimal CenterY { get; set; }
        public decimal RadiusX { get; set; } // Horizontal radius
        public decimal RadiusY { get; set; } // Vertical radius

        public override decimal LowerBoundX {get; protected set;}
        public override decimal UpperBoundX { get; protected set;}
        public override decimal LowerBoundY { get; protected set;}
        public override decimal UpperBoundY { get; protected set;}
        public Ellipse(decimal centerX, decimal centerY, decimal radiusX, decimal radiusY)
        {
            CenterX = centerX;
            CenterY = centerY;
            RadiusX = radiusX;
            RadiusY = radiusY;

            LowerBoundX = Math.Max(0, CenterX - RadiusX);
            UpperBoundX = CenterX + RadiusX;
            LowerBoundY = Math.Max(0, CenterY - RadiusY);
            UpperBoundY = CenterY + RadiusY;
        }

        public override bool ContainsPoint(decimal x, decimal y)
        {
            // Equation of an ellipse: ((x−h)² / rx²) + ((y−k)² / ry²) ≤ 1
            decimal dx = x - CenterX;
            decimal dy = y - CenterY;

            double normX = (double)(dx / RadiusX);
            double normY = (double)(dy / RadiusY);
            
            return (normX * normX + normY * normY) <= 1.0;
        }
    }
}