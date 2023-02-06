using KarteReport.Model.ExportKarte1;
using KarteReport.NameLabel.Models;

namespace KarteReport.Interface
{
    public interface IReportService
    {
        CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

        Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);

    }
}
