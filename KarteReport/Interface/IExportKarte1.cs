using KarteReport.Model.ExportKarte1;

namespace KarteReport.Interface
{
    public interface IExportKarte1
    {
        Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
    }
}
