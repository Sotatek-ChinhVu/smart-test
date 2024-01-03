using System.Drawing;

namespace Reporting.GrowthCurve.Model
{
    public class Line
    {
        public Point From { get; set; }
        public Point To { get; set; }
        public Line()
        {

        }
        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }
    }
}
