namespace Reporting.GrowthCurve.Model
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point From { get; }
        public Point To { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(Point from, Point to)
        {
            From = from;
            To = to;
        }
    }
}
