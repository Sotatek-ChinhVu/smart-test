using Interactor.ExportPDF.Karte2;

namespace Interactor.ExportPDF;

public interface IReporting
{
    MemoryStream PrintKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
    MemoryStream PrintKarte2(Karte2ExportInput inputData);
}
