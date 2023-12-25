namespace Reporting.GrowthCurve.Model
{
    public class Line
    {
        public Point From { get; set; }
        public Point To { get; set; }
        public Line()
        {
            From = new Point(0, 0);
            To = new Point(0, 0);
        }
        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }
    }
}
