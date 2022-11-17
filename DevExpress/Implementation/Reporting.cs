using DevExpress.Inteface;
using Interactor.ExportPDF;
using Interactor.ExportPDF.Karte1;
using Interactor.ExportPDF.Karte2;

namespace DevExpress.Implementation;

public class Reporting : IReporting
{

    private readonly IKarte1Export _karte1Export;
    private readonly IKarte2Export _karte2Export;

    public Reporting(IKarte1Export karte1Export, IKarte2Export karte2Export)
    {
        _karte1Export = karte1Export;
        _karte2Export = karte2Export;
    }

    public Karte1Output PrintKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei)
    {
        return _karte1Export.ExportToPdf(hpId, ptId, sinDate, hokenPid, tenkiByomei);
    }

    public Karte2Output PrintKarte2(Karte2ExportInput inputData)
    {
        return _karte2Export.ExportToPdf(inputData);
    }
}