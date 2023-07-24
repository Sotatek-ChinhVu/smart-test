using Entity.Tenant;

namespace Reporting.GrowthCurve.Model;

public class GcStdInfModel
{
    public GcStdMst GcStdMst { get; private set; }

    public int StdKbn { get => GcStdMst.StdKbn; }
    public int Sex { get => GcStdMst.Sex; }

    public double Point { get => GcStdMst.Point; }

    public double SdM25 { get => GcStdMst.SdM25; }

    public double SdM20 { get => GcStdMst.SdM20; }

    public double SdM10 { get => GcStdMst.SdM10; }

    public double SdP10 { get => GcStdMst.SdP10; }

    public double SdP20 { get => GcStdMst.SdP20; }

    public double SdP25 { get => GcStdMst.SdP25; }

    public double SdAvg { get => GcStdMst.SdAvg; }

    public double Per03 { get => GcStdMst.Per03; }

    public double Per10 { get => GcStdMst.Per10; }

    public double Per25 { get => GcStdMst.Per25; }

    public double Per75 { get => GcStdMst.Per75; }

    public double Per90 { get => GcStdMst.Per90; }

    public double Per97 { get => GcStdMst.Per97; }

    public double Per50 { get => GcStdMst.Per50; }

    public GcStdInfModel(GcStdMst gcStdMst)
    {
        GcStdMst = gcStdMst;
    }
}
