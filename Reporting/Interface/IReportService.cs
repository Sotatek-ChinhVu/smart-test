using Reporting.Karte1.Model;
using Reporting.Mappers;
using Reporting.NameLabel.Models;

namespace Reporting.Interface
{
    public interface IReportService
    {
        CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate);

        Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei);
    }
}
