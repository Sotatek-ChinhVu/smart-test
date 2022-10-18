using Interactor.ExportPDF.Karte1;
using Interactor.ExportPDF.Karte2;

namespace Interactor.ExportPDF;

public interface IReporting
{
    Karte1Output PrintKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
    Karte2Output PrintKarte2(Karte2ExportInput inputData);
}
