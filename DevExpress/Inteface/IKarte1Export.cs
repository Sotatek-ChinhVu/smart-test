using Interactor.ExportPDF.Karte1;

namespace DevExpress.Inteface;

public interface IKarte1Export
{
    Karte1Output ExportToPdf(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
}
