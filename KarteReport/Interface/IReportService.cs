using KarteReport.Karte1.Model;
using KarteReport.NameLabel.Models;

namespace KarteReport.Interface
{
    public interface IReportService
    {
        CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

        CoKarte1Model GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
    }
}
