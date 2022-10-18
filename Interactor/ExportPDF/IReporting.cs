using Interactor.ExportPDF.Karte1;

namespace Interactor.ExportPDF
{
    public interface IReporting
    {
        Karte1Output PrintKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei);
    }
}
