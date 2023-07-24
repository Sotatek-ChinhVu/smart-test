namespace Reporting.GrowthCurve.Model;

public class PointModel
{
    public double X { get; set; }

    public double Y { get; set; }

    public PointModel(double x, double y)
    {
        X = x;
        Y = y;
    }
}
